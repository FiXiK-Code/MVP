using Microsoft.EntityFrameworkCore;
using MVP.ApiModels;
using MVP.Date.API;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class TaskRep : ITask
    {
        private readonly AppDB _appDB;
        private readonly IStaff _staff;

        public TaskRep(AppDB appDB, IStaff staff)
        {
            _appDB = appDB;
            _staff = staff;
        }

        public IEnumerable<Models.Tasks> AllTasks => _appDB.DBTask;

        public IEnumerable<Models.Tasks> TasksProject(string _projentCode) => _appDB.DBTask.Where(i => i.projectCode == _projentCode);//.Include(p => p.projectCode);

        public Models.Tasks GetTask(TasksParameters taskId) => _appDB.DBTask.FirstOrDefault(p => p.id == taskId.id);

        public void addToDB(Tasks task)
        {
            var proj = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode);
            proj.history += $"\nВ проект добавлена задача {task.desc}";
            _appDB.DBTask.Add(task);
            _appDB.SaveChanges();
        }

        public bool redactToDB(//про подзадачи
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
            DateTime finish,
            string session)
        {
            bool red = false;
            if (recipient == null)
            {
                recipient = session != ""? session: null;
                red = _appDB.DBTask.Where(p => p.supervisor == supervisor).Where(p => p.recipient == null).Where(p => p.status == "В работе").Count() < 1
                    && _appDB.DBTask.Where(p => p.recipient == recipient).Where(p => p.status == "В работе").Count() < 1
                    ? true : false;
            }
            else if (recipient != null)
            {
                if(_appDB.DBTask.FirstOrDefault(p => p.id == iid).recipient != recipient) status = "На паузе"; 
                else red = _appDB.DBTask.Where(p => p.supervisor == recipient).Where(p => p.recipient == null).Where(p => p.status == "В работе").Count() < 1
                    && _appDB.DBTask.Where(p => p.recipient == recipient).Where(p => p.status == "В работе").Count() < 1
                    ? true : false;
            }
            if (red || status != "В работе")
            {
                Tasks task = _appDB.DBTask.FirstOrDefault(p => p.id == iid);
                if (task.status == "В работе" && (status == "Выполнена" || status == "На паузе"))
                {
                    task.actualTime += (TimeSpan)(DateTime.Now.AddHours(-5) - task.startWork);
                    task.historyWorc += $"{DateTime.Now.AddHours(-5).Date.ToString(@"dd\.MM\.yyyy")} в работе: {(DateTime.Now.AddHours(-5) - task.startWork).ToString(@"hh\:mm")}\n";
                    Project proj = new Project();
                    try
                    {
                        proj = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode);
                        proj.timeWork += (TimeSpan)(DateTime.Now.AddHours(-5) - task.startWork);
                    }
                    catch (Exception)
                    {
                        proj = null;
                    }
                    _appDB.SaveChanges();
                }
                if (status == "В работе")
                {
                    task.startWork = DateTime.Now.AddHours(-5);
                }
                task.supervisor = supervisor;
                task.date = date;
                task.dedline = dedline;
                task.recipient = recipient;
                task.comment += comment != null? comment+ "\n": null;
                task.plannedTime = plannedTime;
                if (task.status == "Создана" && status == "В работе")
                    task.start = DateTime.Now.AddHours(-5);
                task.status = status;
                if(status == "Выполнена")
                {
                    task.finish = DateTime.Now.AddHours(-5);
                }else task.finish = finish;

                task.liteTask = liteTask == "Задача" ? false : true;
                try
                {
                    task.priority = liteTask == "Задача" ? _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).priority : -1;
                }
                catch (Exception)
                {
                    task.priority = liteTask == "Задача" ? pririty : -1;
                }
               

                _appDB.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool redactStatus(int id, string stat, string session = "")
        {
            var supervisor = _appDB.DBTask.FirstOrDefault(p => p.id == id).supervisor;
            var resip = _appDB.DBTask.FirstOrDefault(p => p.id == id).recipient;

            bool red = false;
            if (resip == null)
            {
                resip = session != "" ? session : null;
                red = _appDB.DBTask.Where(p => p.supervisor == supervisor).Where(p => p.recipient == null).Where(p => p.status == "В работе").Count() < 1
                    && _appDB.DBTask.Where(p => p.recipient == resip).Where(p => p.status == "В работе").Count() < 1
                    ? true : false;
            }
            else if (resip != null)
            {
                red = _appDB.DBTask.Where(p => p.supervisor == resip).Where(p => p.recipient == null).Where(p => p.status == "В работе").Count() < 1
                    && _appDB.DBTask.Where(p => p.recipient == resip).Where(p => p.status == "В работе").Count() < 1
                    ? true : false;
            }

            if (red || stat != "В работе")
            {
                Tasks task = _appDB.DBTask.FirstOrDefault(p => p.id == id);
                if (task.status == "В работе" && (stat == "Выполнена" || stat == "На паузе"))
                {
                    task.actualTime += (TimeSpan)(DateTime.Now.AddHours(-5) - task.startWork);
                    task.historyWorc += $"{DateTime.Now.AddHours(-5).Date.ToString(@"dd\.MM\.yyyy")} в работе: {(DateTime.Now.AddHours(-5) - task.startWork).ToString(@"hh\:mm")}\n";
                    Project proj = new Project();
                    try
                    {
                        proj = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode);
                        proj.timeWork += (TimeSpan)(DateTime.Now.AddHours(-5) - task.startWork);
                    }
                    catch (Exception)
                    {
                        proj = null;
                    }
                    _appDB.SaveChanges();
                }
                if (stat == "В работе")
                {
                    task.startWork = DateTime.Now.AddHours(-5);
                }
                if (task.status == "Создана" && stat == "В работе")
                    task.start = DateTime.Now.AddHours(-5);
                task.recipient = resip;
                task.status = stat;
                try
                {
                    task.priority = task.liteTask == false ? _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).priority : -1;
                }
                catch (Exception)
                {
                    task.priority = task.liteTask == false ? task.priority : -1;
                }

                if (stat == "Выполнена") task.finish = DateTime.Now.AddHours(-5);
                _appDB.SaveChanges();
                return true;
            }
            return false;
        }    
                
        

        // 8 hours
        public async Task timeWork(int idTask)
        {

            var timer = (new TimeSpan(1, 34, 0) - DateTime.Now.TimeOfDay);//.AddHours(-5)
            await Task.Delay(timer);
            await Task.Run(() => 
                redactStatus(idTask, "На паузе")
            );
        }

        public TasksTableReturnModels GetMoreTasks(List<string> staffNames, SessionRoles roleSession, string filterTable = "", bool TaskTable = false)
        {
            List<Tasks> tasksTabbleFilter = new List<Tasks>();
            if (!TaskTable) tasksTabbleFilter = AllTasks.Where(p => staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient)).ToList();

            else tasksTabbleFilter = AllTasks.Where(p => (staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient))
                                    || (p.supervisor == roleSession.SessionName || p.recipient == roleSession.SessionName)).ToList();

            // редактирование возвращаемых задач в зависимости от фильтра (в перспективе передача нескольких фильтров через запятую)
            List<string> staffsDiv = new List<string>();
            if (filterTable != null) foreach (var filter in filterTable.Split(','))///
                {
                    switch (filter)
                    {
                        case "Все задачи":
                            tasksTabbleFilter = AllTasks.ToList();
                            break;
                        case "Задачи отдела управления":
                            foreach (var staff1 in _staff.AllStaffs.Where(p => p.divisionId == 1).ToList())
                            {
                                if (!staffsDiv.Contains(staff1.name)) staffsDiv.Add(staff1.name);
                            }
                            tasksTabbleFilter = AllTasks.Where(p => staffsDiv.Contains(p.supervisor) || staffsDiv.Contains(p.recipient)).ToList();
                            break;
                        case "Задачи отдела проектирования":
                            foreach (var staff1 in _staff.AllStaffs.Where(p => p.divisionId == 2).ToList())
                            {
                                if (!staffsDiv.Contains(staff1.name)) staffsDiv.Add(staff1.name);
                            }
                            tasksTabbleFilter = AllTasks.Where(p => staffsDiv.Contains(p.supervisor) || staffsDiv.Contains(p.recipient)).ToList();
                            break;
                        case "Задачи отдела изысканий":
                            foreach (var staff1 in _staff.AllStaffs.Where(p => p.divisionId == 3).ToList())
                            {
                                if (!staffsDiv.Contains(staff1.name)) staffsDiv.Add(staff1.name);
                            }
                            tasksTabbleFilter = AllTasks.Where(p => staffsDiv.Contains(p.supervisor) || staffsDiv.Contains(p.recipient)).ToList();
                            break;
                    }
            }

            var today = tasksTabbleFilter.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList();

            List <TasksOut> todayOut = new List<TasksOut>();
            foreach(var task in today)
            {
                var outt = new TasksOut
                {
                    id = task.id,
                    code = task.code,
                    desc = task.desc,
                    TaskCodeParent = task.TaskCodeParent,
                    projectCode = task.projectCode,
                    supervisor = task.supervisor,
                    recipient = task.recipient,
                    priority =task.priority,
                    comment = task.comment,
                    plannedTime =task.plannedTime.ToString(@"hh\:mm"),
                    actualTime = task.actualTime.ToString(@"hh\:mm"),
                    start = task.start.ToString(@"dd\.MM\.yyyy HH\:mm\:ss"),
                    finish =task.finish.ToString(@"dd\.MM\.yyyy HH\:mm\:ss"),
                    date = task.date.ToString(@"dd\.MM\.yyyy"),
                    Stage = task.Stage,
                    liteTask =task.liteTask ,
                    status =task.status ,
                    startWork =task.startWork ,
                    creator =task.creator ,
                    historyWorc = task.historyWorc,
                    dedline = task.dedline.ToString(@"dd\.MM\.yyyy HH\:mm\:ss")

                };
                todayOut.Add(outt);
            }



            // выполненные задачи
            var completed = tasksTabbleFilter.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList();

            List<TasksOut> completedOut = new List<TasksOut>();
            foreach (var task in completed)
            {
                var outt = new TasksOut
                {
                    id = task.id,
                    code = task.code,
                    desc = task.desc,
                    TaskCodeParent = task.TaskCodeParent,
                    projectCode = task.projectCode,
                    supervisor = task.supervisor,
                    recipient = task.recipient,
                    priority = task.priority,
                    comment = task.comment,
                    plannedTime = task.plannedTime.ToString(@"hh\:mm"),
                    actualTime = task.actualTime.ToString(@"hh\:mm"),
                    start = task.start.ToString(@"dd\.MM\.yyyy HH\:mm\:ss"),
                    finish = task.finish.ToString(@"dd\.MM\.yyyy HH\:mm\:ss"),
                    date = task.date.ToString(@"dd\.MM\.yyyy"),
                    Stage = task.Stage,
                    liteTask = task.liteTask,
                    status = task.status,
                    startWork = task.startWork,
                    creator = task.creator,
                    historyWorc = task.historyWorc,
                    dedline = task.dedline.ToString(@"dd\.MM\.yyyy HH\:mm\:ss")

                };
                completedOut.Add(outt);
            }
            // будущие задачи 
            var future = tasksTabbleFilter.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList();

            List<TasksOut> futureOut = new List<TasksOut>();
            foreach (var task in future)
            {
                var outt = new TasksOut
                {
                    id = task.id,
                    code = task.code,
                    desc = task.desc,
                    TaskCodeParent = task.TaskCodeParent,
                    projectCode = task.projectCode,
                    supervisor = task.supervisor,
                    recipient = task.recipient,
                    priority = task.priority,
                    comment = task.comment,
                    plannedTime = task.plannedTime.ToString(@"hh\:mm"),
                    actualTime = task.actualTime.ToString(@"hh\:mm"),
                    start = task.start.ToString(@"dd\.MM\.yyyy HH\:mm\:ss"),
                    finish = task.finish.ToString(@"dd\.MM\.yyyy HH\:mm\:ss"),
                    date = task.date.ToString(@"dd\.MM\.yyyy"),
                    Stage = task.Stage,
                    liteTask = task.liteTask,
                    status = task.status,
                    startWork = task.startWork,
                    creator = task.creator,
                    historyWorc = task.historyWorc,
                    dedline = task.dedline.ToString(@"dd\.MM\.yyyy HH\:mm\:ss")

                };
                futureOut.Add(outt);
            }

            TasksTableReturnModels output = new TasksTableReturnModels
            {
                // задачи на чегодня
                today = todayOut,
                // выполненные задачи
                completed = completedOut,
                // будущие задачи 
                future = futureOut
            };

            return output;
        }
    }
} 
