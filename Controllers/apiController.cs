using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP.Date;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using MVP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class apiController : Controller
    {
        private readonly AppDB _appDB;
        private readonly IPost _post;
        private readonly IRole _role;
        private readonly ITask _task;
        private readonly IProject _project;
        private readonly ILogistickTask _logistickTask;
        private readonly ILogisticProject _logistickProject;
        private readonly IStaff _staff;

        public apiController(IPost post, IRole role, ITask task, IProject project, AppDB appDB, ILogistickTask logistick, IStaff staff, ILogisticProject logistickProject)
        {
            _post = post;
            _role = role;
            _task = task;
            _project = project;
            _appDB = appDB;
            _logistickTask = logistick;
            _staff = staff;
            _logistickProject = logistickProject;
        }

        public DateTime redackPriorAndPerenos(string supervisor, DateTime date, TimeSpan plannedTime, string projectCode, string liteTask)// перенос даты задачи в зависимотсти от загруженности дня и приоритета
        {
            var tasksSuper = _appDB.DBTask.Where(p => (p.supervisor == supervisor && p.recipient == null) || p.recipient == supervisor)
                .Where(p => p.status != "Выполнена").Where(p => p.date.Date == date.Date).OrderBy(p => p.plannedTime).ToList();

            var timeWorkDay = new TimeSpan(8, 0, 0);

            // filling array work time in today
            TimeSpan SumTimeTaskToDay = plannedTime;
            var maxPriority = 0;
            foreach (var task in tasksSuper)
            {
                SumTimeTaskToDay += task.plannedTime;
                maxPriority = task.priority > maxPriority ? task.priority : maxPriority;
            }

            // checking time
            if (SumTimeTaskToDay > timeWorkDay)
            {
                if (_appDB.DBProject.FirstOrDefault(p => p.code == projectCode).priority <= maxPriority || liteTask != "Задача")
                {
                    foreach (var task in tasksSuper.OrderBy(p => p.priority).Reverse().OrderBy(p => p.plannedTime))
                    {
                        if ((SumTimeTaskToDay - task.plannedTime) >= timeWorkDay && (task.priority <= maxPriority || liteTask != "Задача"))
                        {
                            _task.redactToDB(liteTask, task.id,
                                    redackPriorAndPerenos(task.supervisor, task.date.AddDays(1), task.plannedTime,
                                        task.projectCode, task.liteTask == false ? "Задача" : "Вне очереди"), task.dedline,
                               task.status, task.comment, task.supervisor, task.recipient, task.priority, task.plannedTime, task.start, task.finish);

                            var tasksSupernew = _appDB.DBTask.Where(p => (p.supervisor == supervisor && p.recipient == null) || p.recipient == supervisor)
                                .Where(p => p.status != "Выполнена").Where(p => p.date.Date == date.Date).OrderBy(p => p.plannedTime).ToList();
                            SumTimeTaskToDay = plannedTime;
                            maxPriority = 0;
                            foreach (var tasknew in tasksSupernew)
                            {
                                SumTimeTaskToDay += tasknew.plannedTime;
                                maxPriority = tasknew.priority > maxPriority ? tasknew.priority : maxPriority;
                            }

                        }
                        if (SumTimeTaskToDay < timeWorkDay) break;
                        if ((SumTimeTaskToDay - task.plannedTime) < timeWorkDay)
                        {
                            _task.redactToDB(liteTask, task.id, redackPriorAndPerenos(task.supervisor, task.date.AddDays(1), task.plannedTime,
                                        task.projectCode, task.liteTask == false ? "Задача" : "Вне очереди"), task.dedline, task.status, task.comment,
                               task.supervisor, task.recipient, task.priority, task.plannedTime, task.start, task.finish);

                            var tasksSupernew = _appDB.DBTask.Where(p => (p.supervisor == supervisor && p.recipient == null) || p.recipient == supervisor)
                                .Where(p => p.status != "Выполнена").Where(p => p.date.Date == date.Date).OrderBy(p => p.plannedTime).ToList();
                            SumTimeTaskToDay = plannedTime;
                            maxPriority = 0;
                            foreach (var tasknew in tasksSupernew)
                            {
                                SumTimeTaskToDay += tasknew.plannedTime;
                                maxPriority = tasknew.priority > maxPriority ? tasknew.priority : maxPriority;
                            }
                        }
                    }
                }
                else
                {
                    return redackPriorAndPerenos(supervisor, date.AddDays(1), plannedTime,
                                        projectCode, liteTask);
                }
            }
            return date;
        }


        ////////// tasks
        [HttpGet]
        public JsonResult tasks(string filterTaskTable = "", int id = -1)// выдает все задачи определенного сотрудника, либо если есть - по фильтру; если есть id - выдает инф по задаче
        {
            if (id != -1) 
            {
                return new JsonResult(_task.GetTask(id));
            }
            else
            {
                // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
                var roleSession = new SessionRoles();
                var sessionCod = "";
                try
                {
                    roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
                    sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
                }
                catch (Exception)
                {
                    return new JsonResult("Не авторизованный запрос!");////////////////
                }

                // составление списка сотрудников в подчинениии у того кто вошел в сессию
                List<string> staffNames = new List<string>();
                staffNames.Add(roleSession.SessionName);
                foreach (var task in _staff.StaffTable(roleSession.SessionRole, sessionCod))
                {
                    if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
                }

                // список задач сотрудников из вышеупомянутого списка
                List<Tasks> tasksTabbleFilter = _task.AllTasks.Where(p => (staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient))
                                    || (p.supervisor == roleSession.SessionName || p.recipient == roleSession.SessionName)).ToList();

                // редактирование возвращаемых задач в зависимости от фильтра (в перспективе передача нескольких фильтров через запятую)
                List<string> staffsDiv = new List<string>();
                foreach (var filter in filterTaskTable.Split(','))///
                {
                    switch (filter)
                    {
                        case "Все задачи":
                            tasksTabbleFilter = _task.AllTasks.ToList();
                            break;
                        case "Задачи отдела управления":
                            foreach (var staff1 in _staff.AllStaffs.Where(p => p.divisionId == 1).ToList())
                            {
                                if (!staffsDiv.Contains(staff1.name)) staffsDiv.Add(staff1.name);
                            }
                            tasksTabbleFilter = _task.AllTasks.Where(p => staffsDiv.Contains(p.supervisor) || staffsDiv.Contains(p.recipient)).ToList();
                            break;
                        case "Задачи отдела проектирования":
                            foreach (var staff1 in _staff.AllStaffs.Where(p => p.divisionId == 2).ToList())
                            {
                                if (!staffsDiv.Contains(staff1.name)) staffsDiv.Add(staff1.name);
                            }
                            tasksTabbleFilter = _task.AllTasks.Where(p => staffsDiv.Contains(p.supervisor) || staffsDiv.Contains(p.recipient)).ToList();
                            break;
                        case "Задачи отдела изысканий":
                            foreach (var staff1 in _staff.AllStaffs.Where(p => p.divisionId == 3).ToList())
                            {
                                if (!staffsDiv.Contains(staff1.name)) staffsDiv.Add(staff1.name);
                            }
                            tasksTabbleFilter = _task.AllTasks.Where(p => staffsDiv.Contains(p.supervisor) || staffsDiv.Contains(p.recipient)).ToList();
                            break;
                    }
                }

                // сборка модели для возвращения
                TasksReturnModels output = new TasksReturnModels
                {
                    // задачи на чегодня
                    today = tasksTabbleFilter.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList(),

                    // выполненные задачи
                    completed = tasksTabbleFilter.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList(),

                    // будущие задачи 
                    future = tasksTabbleFilter.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList()

                };

                return new JsonResult(output);
            }
            
        }

        [HttpPost]
        public JsonResult PostTasks
            (string code,
            string desc,
            string projectCode,
            string supervisor,
            string recipient,
            string comment,
            TimeSpan plannedTime,
            DateTime date,
            DateTime dedline,
            string Stage,
            string liteTask)//добавляет задачу в базу
        {
            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return new JsonResult("Не авторизованный запрос!");////////////////
            }

            // корректировка даты - автоперенос при заполненном дне
            date = redackPriorAndPerenos(supervisor, date, plannedTime, projectCode, liteTask);

            // добавление задачи в базу
            var item = new Tasks
            {
                actualTime = TimeSpan.Zero,
                desc = desc,
                projectCode = projectCode,
                supervisor = supervisor,
                recipient = recipient,
                priority = liteTask == "Задача" ? _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).priority : -1,
                comment = comment != null ? $"{roleSession.SessionName}: {comment}\n" : null,
                plannedTime = plannedTime,
                date = date,
                dedline = dedline,
                Stage = Stage,
                status = "Создана",
                liteTask = liteTask == "Задача" ? false : true,
                creator = roleSession.SessionName

            };
            _task.addToDB(item);

            // заполнение лога
            var iid = _appDB.DBTask.FirstOrDefault(p => p.desc == desc).id;
            LogistickTask log = new LogistickTask()
            {
                ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                TaskId = iid,
                descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                dateRedaction = DateTime.Now,
                planedTime = plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == "Создана").id,
                comment = "Задача создана. Комментарий: " + comment
            };
            _logistickTask.addToDB(log);


            return new JsonResult("Задача добавлена!");
        }

        [HttpPut]
        public JsonResult PutTasks(
             string liteTask,
            int iid,
            DateTime date,
            DateTime dedline,
            string status,
            string comment,
            string supervisor,
            string recipient,
            int pririty,
            TimeSpan plannedTime,
            DateTime start,
            DateTime finish)// обновляет задачу
        {

            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return new JsonResult("Не авторизованный запрос!");////////////////
            }

            // корректировка даты - автоперенос при заполненном дне
            date = redackPriorAndPerenos(supervisor, date, plannedTime, _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode, liteTask);

            // попытка редактирования задачи
            if (!_task.redactToDB(liteTask, iid, date, dedline, status, comment != null ? $"{roleSession.SessionName}: {comment}\n" : null, supervisor, recipient, pririty, plannedTime, start, finish))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return new JsonResult(msg);////////////////
            }
            else
            {
                // проверка перехода проекта в следующую стадию
                var projCod = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode;
                var projId = _appDB.DBProject.FirstOrDefault(p => p.code == projCod) != null ? _appDB.DBProject.FirstOrDefault(p => p.code == projCod).id : -1;
                _project.NextStage(projId);

                // заполнение лога
                LogistickTask item = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                    TaskId = iid,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                    resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                    dateRedaction = DateTime.Now,
                    planedTime = plannedTime,
                    actualTime = _appDB.DBTask.FirstOrDefault(p => p.id == iid).actualTime,
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == status).id,
                    comment = comment
                };
                _logistickTask.addToDB(item);


                return new JsonResult("");
            }
        }

        //////// ????
        [HttpDelete]
        public JsonResult DeleteTasks()// удаляет задачу???
        {
            return new JsonResult("");
        }

        [HttpGet]
        public JsonResult GetSearch()// поиск чего?
        {
            return new JsonResult("");
        }

        ////////// projects
        [HttpGet]
        public JsonResult GetProjects(int id = -1)// список проектов, если есть id - выдает инф по проекту
        {
            if (id != -1)
            {
                return new JsonResult(_project.GetProject(id)); 
            }
            else return new JsonResult(_project.AllProjects);
        }

        [HttpGet]
        public JsonResult GetEmployees(bool all = false)// список сотрудников
        {
            
            if (!all)
            {
                // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
                var roleSession = new SessionRoles();
                var sessionCod = "";
                try
                {
                    roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
                    sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
                }
                catch (Exception)
                {
                    return new JsonResult("Не авторизованный запрос!");////////////////
                }

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(_staff.StaffTable(roleSession.SessionRole, sessionCod));
            }
            // возвращает всех сотрудников
            else return new JsonResult(_staff.AllStaffs);
        }
    }
}
