using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVP.ApiModels;
using MVP.Date;
using MVP.Date.API;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using MVP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ApiController : Controller
    {
        private readonly AppDB _appDB;
        private readonly IPost _post;
        private readonly IRole _role;
        private readonly ITask _task;
        private readonly IProject _project;
        private readonly ILogistickTask _logistickTask;
        private readonly ILogisticProject _logistickProject;
        private readonly IStaff _staff;

        public ApiController(IPost post, IRole role, ITask task, IProject project, AppDB appDB, ILogistickTask logistick, IStaff staff, ILogisticProject logistickProject)
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

        public class AuthOptions
        {
            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена
            const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
            public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }

        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] IdentityPerson person)// генерация токена
        {
            var identity = await GetIdentity(person.UserName, person.Password); // проверка на авторизацию 
            if (identity == null)
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity,
                    expires: now.Add(TimeSpan.FromMinutes(480)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(encodedJwt);
        }
        // функция проверки
        private async Task<IReadOnlyCollection<Claim>> GetIdentity(string userName, string password)
        {
            List<Claim> claims = null;
            var user = _appDB.DBStaff.FirstOrDefault(p => p.name == userName && p.passvord == password);
            if (user != null)
            {
               // var sha256 = new SHA256Managed();
               // var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                if (password == user.passvord)
                {
                    claims = new List<Claim>
                    { 
                        new Claim(ClaimsIdentity.DefaultNameClaimType, _appDB.DBStaff.FirstOrDefault(p => p.name == userName).login),
                    };
                }
            }
            return claims;
        }

        // редактирование даты постановки задачи в зависимоти от загруженности для
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
                               task.status, task.comment, task.supervisor, task.recipient, task.priority, task.plannedTime, task.start, task.finish, "");

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
                               task.supervisor, task.recipient, task.priority, task.plannedTime, task.start, task.finish,"");

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
        //[Authorize]
        [HttpGet]
        public JsonResult GetTasks([FromQuery] TasksParameters TaskParam)// выдает все задачи определенного сотрудника, либо если есть - по фильтру; если есть id - выдает инф по задаче
        { 
            if (TaskParam.id != -1) 
            {
                return new JsonResult(JsonConvert.SerializeObject(_task.GetTask(TaskParam)));
            }
            else
            {
                // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
                var roleSession = new SessionRoles();
                var sessionCod = "";
                try
                {
                    var post = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.personName).post;
                    var roleId = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                    var sessionInit = new SessionRoles()
                    {
                        SessionName = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.personName).name,
                        SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleId).name
                    };
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
                List<Tasks> tasksTabbleFilter = _task.GetMoreTasks(staffNames, roleSession, TaskParam.filterTable, true);

                // сборка модели для возвращения
                TasksTableReturnModels output = new TasksTableReturnModels
                {
                    // задачи на чегодня
                    today = tasksTabbleFilter.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList(),

                    // выполненные задачи
                    completed = tasksTabbleFilter.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList(),

                    // будущие задачи 
                    future = tasksTabbleFilter.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList()

                };

                return new JsonResult(JsonConvert.SerializeObject(output));
            }
            
        }

        //[Authorize]
        [HttpPost]
        public JsonResult PostTasks
            ([FromQuery] TasksParameters TaskParam)//добавляет задачу в базу
        {
            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var post = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.personName).post;
                var roleId = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                var sessionInit = new SessionRoles()
                {
                    SessionName = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.personName).name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleId).name
                };
                sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
            }
            catch (Exception)
            {
                return new JsonResult("Не авторизованный запрос!");////////////////
            }

            // корректировка даты - автоперенос при заполненном дне
            TaskParam.date = redackPriorAndPerenos(TaskParam.supervisor, TaskParam.date, TaskParam.plannedTime, TaskParam.projectCode, TaskParam.liteTask);

            // добавление задачи в базу
            var item = new Tasks
            {
                actualTime = TimeSpan.Zero,
                desc = TaskParam.desc,
                projectCode = TaskParam.projectCode,
                supervisor = TaskParam.supervisor,
                recipient = TaskParam.recipient,
                priority = TaskParam.liteTask == "Задача" ? _appDB.DBProject.FirstOrDefault(p => p.code == TaskParam.projectCode).priority : -1,
                comment = TaskParam.comment != null ? $"{roleSession.SessionName}: {TaskParam.comment}\n" : null,
                plannedTime = TaskParam.plannedTime,
                date = TaskParam.date,
                dedline = TaskParam.dedline,
                Stage = TaskParam.Stage,
                status = "Создана",
                liteTask = TaskParam.liteTask == "Задача" ? false : true,
                creator = roleSession.SessionName

            };
            _task.addToDB(item);

            // заполнение лога
            var iid = _appDB.DBTask.FirstOrDefault(p => p.desc == TaskParam.desc).id;
            LogistickTask log = new LogistickTask()
            {
                ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                TaskId = iid,
                descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.supervisor).id,
                resipienId = TaskParam.supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.supervisor).id : -1,
                dateRedaction = DateTime.Now,
                planedTime = TaskParam.plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == "Создана").id,
                comment = "Задача создана. Комментарий: " + TaskParam.comment
            };
            _logistickTask.addToDB(log);


            return new JsonResult("Задача добавлена!");
        }

        //[Authorize]
        [HttpPut]
        public JsonResult PutTasks
            ([FromQuery] TasksParameters TaskParam)// обновляет задачу
        {

            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var post = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.personName).post;
                var roleId = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                var sessionInit = new SessionRoles()
                {
                    SessionName = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.personName).name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleId).name
                };
                sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
            }
            catch (Exception)
            {
                return new JsonResult("Не авторизованный запрос!");////////////////
            }

            // корректировка даты - автоперенос при заполненном дне
            TaskParam.date = redackPriorAndPerenos(TaskParam.supervisor, TaskParam.date, TaskParam.plannedTime, _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode, TaskParam.liteTask);

            // попытка редактирования задачи
            if (!_task.redactToDB(TaskParam.liteTask, TaskParam.id, TaskParam.date, TaskParam.dedline, TaskParam.status, TaskParam.comment != null ? $"{roleSession.SessionName}: {TaskParam.comment}\n" : null, TaskParam.supervisor, TaskParam.recipient,TaskParam. pririty, TaskParam.plannedTime, TaskParam.start, TaskParam.finish, roleSession.SessionName))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return new JsonResult(msg);////////////////
            }
            else // при успешном редактировании ->
            {
                // проверка перехода проекта в следующую стадию
                var projCod = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode;
                var projId = _appDB.DBProject.FirstOrDefault(p => p.code == projCod) != null ? _appDB.DBProject.FirstOrDefault(p => p.code == projCod).id : -1;
                _project.NextStage(projId);

                // заполнение лога
                LogistickTask item = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode,
                    TaskId = TaskParam.id,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.supervisor).id,
                    resipienId = TaskParam.supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == TaskParam.supervisor).id : -1,
                    dateRedaction = DateTime.Now.AddHours(-5),
                    planedTime = TaskParam.plannedTime,
                    actualTime = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).actualTime,
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == TaskParam.status).id,
                    comment = TaskParam.comment
                };
                _logistickTask.addToDB(item);


                return new JsonResult("Задача успешно обновлена!");
            }
        }

        //////// ????
        //[Authorize]
        //[HttpDelete]
        //public JsonResult DeleteTasks()// удаляет задачу???
        //{
        //    return new JsonResult("");
        //}

        //[Authorize]
        [HttpGet]
        public JsonResult GetSearch(string param)// поиск по описанию задачи - возвраащет список задач в которых описание содержит передаваемый текст
        {
            return new JsonResult(_appDB.DBTask.Where(p => p.desc.Contains(param)).ToList());
        }

        ////////// projects
        //[Authorize]
        [HttpGet]
        public JsonResult GetProjects([FromQuery] ProjectParameters ProjParam)// список проектов + 2 фильтра: в архиве или текущие, все гипы или выборочно, если есть id - выдает инф по проекту
        {
            // проверка сесии
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var post = _appDB.DBStaff.FirstOrDefault(p => p.name == ProjParam.personName).post;
                var roleId = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                var sessionInit = new SessionRoles()
                {
                    SessionName = _appDB.DBStaff.FirstOrDefault(p => p.name == ProjParam.personName).name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleId).name
                };
                sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
            }
            catch (Exception)
            {
                return new JsonResult("Не авторизованный запрос!");////////////////
            }

            if (ProjParam.id != -1)
            {
                // возвращает инфу по id проекта
                return new JsonResult(_project.GetProject(ProjParam.id));
            }
            else
            {
                var projects = _project.AllProjects;

                // фильтрация архивных проектов
                foreach (var filter in ProjParam.filterProj.Split(','))
                {
                    if (filter == "Проекты в архиве")
                    {
                        projects = projects.Where(p => p.archive == "Да");
                    }
                    if (filter == "Текущие проекты")
                    {
                        projects = projects.Where(p => p.archive == "Нет");
                    }
                }

                // фильтрация по ответсвенному за проекты
                foreach (var filter in ProjParam.supervisorFilter.Split(','))
                {
                    if (filter != "Все ГИПы" && filter != "")
                    {
                        projects = projects.Where(p => p.supervisor == filter);
                    }
                }

                ProjectTableReturnModels output = new ProjectTableReturnModels
                {
                    // проекты
                    projects = projects.ToList(),
                    // задачи на чегодня
                    today = _task.AllTasks.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList(),

                    // выполненные задачи
                    completed = _task.AllTasks.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList(),

                    // будущие задачи 
                    future = _task.AllTasks.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList()

                };

                return new JsonResult(output);
            }
        }

        //[Authorize]
        [HttpGet]
        public JsonResult GetEmployees([FromQuery] StaffParameters StaffParam)// список сотрудников
        {
            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var post = _appDB.DBStaff.FirstOrDefault(p => p.name == StaffParam.personName).post;
                var roleId = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                var sessionInit = new SessionRoles()
                {
                    SessionName = _appDB.DBStaff.FirstOrDefault(p => p.name == StaffParam.personName).name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleId).name
                };
                sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
            }
            catch (Exception)
            {
                return new JsonResult("Не авторизованный запрос!");////////////////
            }

            // списка сотрудников без фильтрации
            if (StaffParam.filterStaff == "")
            {
                List<string> staffNames = new List<string>();
                staffNames.Add(roleSession.SessionName);
                foreach (var task in _staff.StaffTable(roleSession.SessionRole, sessionCod))
                {
                    if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
                }

                List<Tasks> tasksTabbleFilter = _task.GetMoreTasks(staffNames, roleSession, "");

                StaffTableReturnModels output = new StaffTableReturnModels
                {
                    // список сотрудников
                    staffs = _staff.StaffTable(roleSession.SessionRole, sessionCod).ToList(),
                    // задачи на чегодня
                    today = tasksTabbleFilter.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList(),

                    // выполненные задачи
                    completed = tasksTabbleFilter.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList(),

                    // будущие задачи 
                    future = tasksTabbleFilter.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList()

                };

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(output);
            }
            // список сотрудников с фильтром по должности
            else
            {
                var StaffTable = _staff.StaffTable(roleSession.SessionRole, sessionCod);

                foreach (var filter in StaffParam.filterStaff.Split(','))
                {
                    if (filter != "" && filter != "Все должности")
                    {
                        StaffTable = StaffTable.Where(p => p.post == filter).ToList();
                    }
                }

                // составление списка сотрудников в подчинениии у того кто вошел в сессию
                List<string> staffNames = new List<string>();
                staffNames.Add(roleSession.SessionName);
                foreach (var task in StaffTable)
                {
                    if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
                }

                // список задач сотрудников из вышеупомянутого списка
                List<Tasks> tasksTabbleFilter = _task.GetMoreTasks(staffNames, roleSession, "", true);

                StaffTableReturnModels output = new StaffTableReturnModels
                {
                    staffs = StaffTable,
                    // задачи на чегодня
                    today = tasksTabbleFilter.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList(),

                    // выполненные задачи
                    completed = tasksTabbleFilter.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList(),

                    // будущие задачи 
                    future = tasksTabbleFilter.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList()
                };

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(output);
            }
        }
    }
}
