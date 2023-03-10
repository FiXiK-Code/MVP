using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MVP.ApiModels;
using MVP.Date;
using MVP.Date.API;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using MVP.SignalR;
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
    public class ApiController : Controller
    {
        private readonly AppDB _appDB;
        private readonly IHubContext<MyHub> _hub;
        private readonly IPost _post;
        private readonly IRole _role;
        private readonly ITask _task;
        private readonly IProject _project;
        private readonly ILogistickTask _logistickTask;
        private readonly ILogisticProject _logistickProject;
        private readonly IStaff _staff;

        public ApiController(IPost post, IRole role, ITask task, IProject project,
            AppDB appDB, ILogistickTask logistick, IStaff staff, ILogisticProject logistickProject,
            IHubContext<MyHub> hub)
        {
            _hub = hub;
            _post = post;
            _role = role;
            _task = task;
            _project = project;
            _appDB = appDB;
            _logistickTask = logistick;
            _staff = staff;
            _logistickProject = logistickProject;
        }

        public class AuthOptions//вспомогательный класс для генерации токена
        {
            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена
            const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
            public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }

        [HttpPost]
        public IActionResult Token([FromBody] IdentityPerson person)// генерация токена
        {
            var identity = GetIdentity(person.UserName, person.Password); // проверка на авторизацию 
            if (identity == null)
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(480)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var personbuf = _appDB.DBStaff.FirstOrDefault(p => p.login == person.UserName);
            var role = _appDB.DBRole.FirstOrDefault(p => p.code == personbuf.roleCod).name;

            var response = new
            {
                access_token = encodedJwt,
                user_login = identity.Name,
                user_name = personbuf.name,
                user_role = role,
                user_id = personbuf.id
            };

            return Ok(response);
        }
        
        private ClaimsIdentity GetIdentity(string userName, string password)//фeнкция проверки
        {
            List<Claim> claims = null;
            var user = _appDB.DBStaff.FirstOrDefault(p => p.login == userName && p.passvord == password);
            if (user != null)
            {
                // var sha256 = new SHA256Managed();
                // var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                if (password == user.passvord)
                {
                    claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, _appDB.DBStaff.FirstOrDefault(p => p.login == userName).login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, _appDB.DBStaff.FirstOrDefault(p => p.login == userName).roleCod)

                    };
                }
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                   ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        [HttpGet]
        public JsonResult EndWorkDay()//ставит все задачи на паузу в конце рабочего дня
        {
            _task.timeWork();
            return new JsonResult(new ObjectResult("Задачи перенесены в статус \"На паузе\"!") { StatusCode = 201 });
        }


        public PerenosContext redackPriorAndPerenos(string status,string supervisor, DateTime date, TimeSpan plannedTime,
            string projectCode, bool liteTask, List<Tasks> tasks = null)// перенос даты задачи в зависимотсти от загруженности дня и приоритета
        {
            if (status == "Выполнена") {
                var outputt = new PerenosContext
                {
                    date = date,
                    tasks = new List<Tasks>()
                };
                return outputt;
            }
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
            var output = new PerenosContext
            {
                date = date,
                tasks = new List<Tasks>()
            };
            if (tasks!= null) output.tasks.AddRange(tasks);
            // checking time
            if (SumTimeTaskToDay > timeWorkDay)
            {
                if (_appDB.DBProject.FirstOrDefault(p => p.code == projectCode).priority <= maxPriority || liteTask != false)
                {
                    foreach (var task in tasksSuper.OrderBy(p => p.priority).Reverse().OrderBy(p => p.plannedTime))
                    {
                        if ((SumTimeTaskToDay - task.plannedTime) >= timeWorkDay && (task.priority <= maxPriority || liteTask != false))
                        {
                            var buf = redackPriorAndPerenos("",task.recipient == null ? task.supervisor : task.recipient, task.date.AddDays(1), task.plannedTime,
                                        task.projectCode, task.liteTask, output.tasks);

                            foreach(var buffTask in buf.tasks)
                            {
                                if (output.tasks.Count != 0)
                                {
                                    var mas = output.tasks.Where(p => p.id == buffTask.id).ToList();
                                    foreach (var taskk in mas)
                                    {
                                        output.tasks.Remove(taskk);
                                        if (output.tasks.Count == 0) break;
                                    }
                                }
                                output.tasks.Add(buffTask);
                            }

                            _task.redactToDB(liteTask, task.id,
                                    buf.date,
                                    task.dedline, task.status, task.comment, task.supervisor, task.recipient, task.priority,
                                    task.plannedTime, task.start, task.finish, "");

                            if (_appDB.DBTask.FirstOrDefault(p => p.id == task.id).date.Date != date)
                            {
                                if (output.tasks.Count != 0)
                                {
                                    var mas = output.tasks.Where(p => p.id == task.id).ToList();
                                    foreach (var taskk in mas)
                                    {
                                        output.tasks.Remove(taskk);
                                        if (output.tasks.Count == 0) break;
                                    }
                                }
                                output.tasks.Add(task);
                            }

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
                            var buf = redackPriorAndPerenos("", task.recipient == null ? task.supervisor : task.recipient, task.date.AddDays(1), task.plannedTime,
                                        task.projectCode, task.liteTask, output.tasks);

                            foreach (var buffTask in buf.tasks)
                            {
                                if (output.tasks.Count != 0)
                                {
                                    var mas = output.tasks.Where(p => p.id == buffTask.id).ToList();
                                    foreach (var taskk in mas)
                                    {
                                        output.tasks.Remove(taskk);
                                        if (output.tasks.Count == 0) break;
                                    }
                                }
                                output.tasks.Add(buffTask);
                            }

                            _task.redactToDB(liteTask, task.id,
                                    buf.date,
                                task.dedline, task.status, task.comment, task.supervisor, task.recipient, task.priority,
                                task.plannedTime, task.start, task.finish, "");

                            if (_appDB.DBTask.FirstOrDefault(p => p.id == task.id).date.Date != date)
                            {
                                if (output.tasks.Count != 0)
                                {
                                    var mas = output.tasks.Where(p => p.id == task.id).ToList();
                                    foreach (var taskk in mas)
                                    {
                                        output.tasks.Remove(taskk);
                                        if (output.tasks.Count == 0) break;
                                    }
                                }
                                output.tasks.Add(task);
                            }

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
                    return redackPriorAndPerenos("",supervisor, date.AddDays(1), plannedTime,
                                        projectCode, liteTask, output.tasks);
                }
            }
            return output;
        }

        ////////////////// TASKS
        [Authorize]
        [HttpGet]
        public JsonResult GetTasks([FromQuery] TasksParameters TaskParam)// выдает все задачи определенного сотрудника, либо если есть - по фильтру; если есть id - выдает инф по задаче
        {
            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("not authorized!") { StatusCode = 401 });////////////////
            }
            //_hub.Clients.All.SendAsync("ResiveMassage", "Test");
            if (TaskParam.id != -1)
            {
                Tasks result = new Tasks();
                try
                {
                    result = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id);
                }
                catch (Exception)
                {
                    result = null;
                }

                if (result == null) return new JsonResult(new ObjectResult("task not found") { StatusCode = 404 });
                return new JsonResult(new ObjectResult(_task.GetTask(TaskParam)) { StatusCode = 200 });
                //return new JsonResult(JsonConvert.SerializeObject(_task.GetTask(TaskParam)));
            }
            else
            {
                // составление списка сотрудников в подчинениии у того кто вошел в сессию
                List<string> staffNames = new List<string>();
                staffNames.Add(roleSession.SessionName);
                foreach (var task in _staff.StaffTable(roleSession.SessionRole, sessionCod))
                {
                    if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
                }

                TasksTableReturnModels output = _task.GetMoreTasks(staffNames, roleSession, TaskParam.filterTasks);

                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult PostTasks([FromBody] TasksParameters TaskParam)//добавляет задачу в базу
        {
            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
                
            }
            if (TaskParam.desc.Length < 1 ||
                    TaskParam.projectCode == -1 ||
                    TaskParam.supervisor == -1 ||
                    TaskParam.plannedTime.Length < 1 ||
                    TaskParam.date.Length < 1 ||
                    TaskParam.dedline.Length < 1)
            {
                string contentError = "";
                if (TaskParam.desc.Length < 1) contentError += "Описание задачи; ";
                if (TaskParam.projectCode == -1) contentError += "Код проекта; ";
                if (TaskParam.supervisor == -1) contentError += "Ответственный по задаче; ";
                if (TaskParam.plannedTime.Length < 1) contentError += "Планируемое время исполнения; ";
                if (TaskParam.date.Length < 1) contentError += "Дата; ";
                if (TaskParam.dedline.Length < 1) contentError += "Дедлайн; ";

                var error = new
                {
                    messsage = "Не все поля заполнены!",
                    content = "Не заполнены поля: " + contentError
                };
                return new JsonResult(new ObjectResult(error) { StatusCode = 400 });
            }

            string supervisor = null;
            try 
            { 
                supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.supervisor).name; 
            }catch (Exception) { return new JsonResult(new ObjectResult("Указанный ответственный не найден!") { StatusCode = 404 }); }

            string recipient = null;
            try
            {
                recipient = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.recipient).name;
            }catch (Exception) { }

            var date = DateTime.Parse(TaskParam.date);
            var plannedTime = TimeSpan.Parse(TaskParam.plannedTime);
            var dedline = DateTime.Parse(TaskParam.dedline);

            string projectCode = null;
            try
            {
                projectCode = _appDB.DBProject.FirstOrDefault(p => p.id == TaskParam.projectCode).code;
            }catch (Exception) { return new JsonResult(new ObjectResult($"Проект c id {TaskParam.projectCode} - не найден!") { StatusCode = 404 });}
            
            var redact = redackPriorAndPerenos("", recipient == null? supervisor : recipient, date, plannedTime, projectCode, TaskParam.liteTask);
            date = redact.date;

            //_hub.Clients.All.SendAsync("ResiveMassage", "Test");
            // добавление задачи в базу
            var item = new Tasks
            {
                actualTime = TimeSpan.Zero,
                desc = TaskParam.desc,
                projectCode = projectCode,
                supervisor = supervisor,
                recipient = recipient,
                priority = TaskParam.liteTask == false ? _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).priority : -1,
                comment = TaskParam.comment.Length > 1 ? $"{roleSession.SessionName}: {TaskParam.comment}\n" : null,
                plannedTime = plannedTime,
                date = date,
                dedline = dedline,
                Stage = TaskParam.Stage,
                status = "Создана",
                liteTask = TaskParam.liteTask,
                creator = roleSession.SessionName

            };
            _task.addToDB(item);

            // заполнение лога
            var task = item;
            LogistickTask log = new LogistickTask()
            {
                ProjectCode = task.projectCode,
                TaskId = task.id,
                descTask = task.desc,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                dateRedaction = DateTime.Now.AddHours(-5),
                planedTime = plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == "Создана").id,
                comment = TaskParam.comment.Length > 1 ? "Задача создана. Комментарий: " + TaskParam.comment : "Задача создана"
            };
            _logistickTask.addToDB(log);

            var typeTasks = item.date.Date > DateTime.Now.Date ? "future" : "today";
            typeTasks = item.status == "Выполнена" ? "completed" : typeTasks;
            var taskOut = new TasksOut
            {
                id = task.id,
                code = task.code,
                desc = task.desc,
                TaskCodeParent = task.TaskCodeParent,
                projectCode = task.projectCode,
                projectId = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).id,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.supervisor).id,
                recipientId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.recipient).id,
                supervisor = task.supervisor,
                recipient = task.recipient,
                priority = task.priority,
                comment = task.comment,
                plannedTime = task.plannedTime.ToString(@"hh\:mm"),
                actualTime = task.actualTime.ToString(@"hh\:mm"),
                start = task.start,
                finish = task.finish,
                date = task.date.ToString(@"yyyy\-MM\-dd"),
                Stage = task.Stage,
                liteTask = task.liteTask,
                status = task.status,
                startWork = task.startWork,
                creator = task.creator,
                historyWorc = task.historyWorc,
                dedline = task.dedline,
                creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id

            };
            var outt = new {
            message = "Задача создана!",
            value = taskOut,
            type = typeTasks
            };
            return new JsonResult(new ObjectResult(outt) { StatusCode = 201 });
        }

        [Authorize]
        [HttpPut]
        public JsonResult PutTasks([FromBody] TasksParameters TaskParam)// обновляет задачу
        {
            Tasks result = new Tasks();
            try
            {
                result = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id);
            }
            catch (Exception) {}

            if (result == new Tasks()) return new JsonResult(new ObjectResult("task not found") { StatusCode = 404 });

            
            if (TaskParam.id == -1 ||
                   TaskParam.date.Length < 1 ||
                   TaskParam.supervisor == -1 ||
                   TaskParam.date.Length < 1 ||
                   TaskParam.plannedTime.Length < 1)
            {
                string contentError = "";
                if (TaskParam.id == -1) contentError += "Id задачи; ";
                if (TaskParam.date.Length < 1) contentError += "Дата; ";
                if (TaskParam.supervisor == -1) contentError += "Ответственный по задаче; ";
                if (TaskParam.plannedTime.Length < 1) contentError += "Планируемое время исполнения; ";
                var error = new
                {
                    messsage = "Не все поля заполнены!",
                    content = "Не заполнены поля: " + contentError
                };
                return new JsonResult(new ObjectResult(error) { StatusCode = 400 });
            }

            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception) { return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 }); }

            if (result.date.Date != DateTime.Parse(TaskParam.date).Date && result.supervisor != roleSession.SessionName)
            {
                return new JsonResult(new ObjectResult($"Невозможно перенести дату! Перенести может только {result.supervisor}") { StatusCode = 403 });
            }

            string supervisor = null;
            try
            {
                supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.supervisor).name;
            }
            catch (Exception) { return new JsonResult(new ObjectResult("Указанный ответственный не найден!") { StatusCode = 404 }); }

            string recipient = null;
            try
            {
                recipient = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.recipient).name;
            }
            catch (Exception) { }

            var date = DateTime.Parse(TaskParam.date);
            var plannedTime = TimeSpan.Parse(TaskParam.plannedTime);
            var start = DateTime.Parse(TaskParam.start);
            var finish = DateTime.Parse(TaskParam.finish);
            var dedline = DateTime.Parse(TaskParam.dedline);

            string projectCode = null;
            try
            {
                projectCode = _appDB.DBProject.FirstOrDefault(p => p.id == TaskParam.projectCode).code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult($"Проект c id {TaskParam.projectCode} - не найден!") { StatusCode = 404 });
            }
           
            
            // корректировка даты - автоперенос при заполненном дне
            var test = redackPriorAndPerenos(TaskParam.status, supervisor, date, plannedTime, _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode, TaskParam.liteTask);
            date = test.date;

            //_hub.Clients.All.SendAsync("ResiveMassage", "Test");

            // попытка редактирования задачи
            if (!_task.redactToDB(TaskParam.liteTask, TaskParam.id, date, dedline, TaskParam.status, TaskParam.comment.Length < 1 ? $"{roleSession.SessionName}: {TaskParam.comment}\n" : null, supervisor, recipient, TaskParam.pririty, plannedTime, start, finish, roleSession.SessionName))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return new JsonResult(new ObjectResult(msg) { StatusCode = 403 });////////////////
            }
            else // при успешном редактировании ->
            {
                // проверка перехода проекта в следующую стадию
                var projCod = "";
                try
                {
                    projCod = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode;
                }
                catch (Exception)
                {
                    return new JsonResult(new ObjectResult($"Проект, указанный в задаче, не найден!") { StatusCode = 404 });
                }
                var projId = _appDB.DBProject.FirstOrDefault(p => p.code == projCod) != null ? _appDB.DBProject.FirstOrDefault(p => p.code == projCod).id : -1;
                _project.NextStage(projId);

                // заполнение лога
                LogistickTask item = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode,
                    TaskId = TaskParam.id,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                    resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                    dateRedaction = DateTime.Now.AddHours(-5),
                    planedTime = plannedTime,
                    actualTime = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).actualTime,
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == TaskParam.status).id,
                    comment = TaskParam.comment.Length > 1 ? TaskParam.comment : null
                };
                _logistickTask.addToDB(item);
                var task = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id);
                var typeTasks = task.date.Date > DateTime.Now.Date ? "future" : "today";
                typeTasks = task.status == "Выполнена" ? "completed" : typeTasks;
                var taskOut = new TasksOut
                {
                    id = task.id,
                    code = task.code,
                    desc = task.desc,
                    TaskCodeParent = task.TaskCodeParent,
                    projectCode = task.projectCode,
                    projectId = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).id,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.supervisor).id,
                    recipientId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.recipient).id,
                    supervisor = task.supervisor,
                    recipient = task.recipient,
                    priority = task.priority,
                    comment = task.comment,
                    plannedTime = task.plannedTime.ToString(@"hh\:mm"),
                    actualTime = task.actualTime.ToString(@"hh\:mm"),
                    start = task.start,
                    finish = task.finish,
                    date = task.date.ToString(@"yyyy\-MM\-dd"),
                    Stage = task.Stage,
                    liteTask = task.liteTask,
                    status = task.status,
                    startWork = task.startWork,
                    creator = task.creator,
                    historyWorc = task.historyWorc,
                    dedline = task.dedline,
                    creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id

                };
                var outt = new
                {
                    message = "Задача успешно обновлена!",
                    value = taskOut,
                    type = typeTasks
                };
                return new JsonResult(new ObjectResult(outt) { StatusCode = 202 });
            }
        }


        [Authorize]
        [HttpPut]
        public JsonResult PutTasksStatus([FromBody] TasksParameters TaskParam)// обновляет статус задачи
        {
            Tasks result = new Tasks();
            try
            {
                result = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id);
            }
            catch (Exception)
            {
                result = null;
            }

            if (result == null) return new JsonResult(new ObjectResult("task not found") { StatusCode = 404 });

            if (TaskParam.id == -1 || TaskParam.status.Length < 1 )
            {
                string contentError = "";
                if (TaskParam.id == -1) contentError += "Id задачи; ";
                if (TaskParam.status.Length < 1) contentError += "Статус задачи; ";

                var error = new
                {
                    messsage = "Не все поля заполнены!",
                    content = "Не заполнены поля: " + contentError
                };
                return new JsonResult(new ObjectResult(error) { StatusCode = 400 });
            }

            var roleSession = new SessionRoles();
            var sessionCod = "";
            Staff person = new Staff();
            try
            {
                person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
                
            }
            
            if (_task.GetTask(TaskParam).recipient != roleSession.SessionName && _task.GetTask(TaskParam).recipient != null)
            {
                var msg = "Нельзя менять статус чужих задач!";
                return new JsonResult(new ObjectResult(msg) { StatusCode = 403 });
            }
            if (!_task.redactStatus(TaskParam.id, TaskParam.status, roleSession.SessionName))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return new JsonResult(new ObjectResult(msg) { StatusCode = 403 });
            }

            var supervisor = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).supervisor;
            LogistickTask item = new LogistickTask()
            {
                ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode,
                TaskId = TaskParam.id,
                descTask = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).desc,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                dateRedaction = DateTime.Now.AddHours(-5),
                planedTime = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).status).id,
                comment = $"Стату задачи изменен на: {TaskParam.status}"
            };
            _logistickTask.addToDB(item);
            var task = _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id);
            var typeTasks = task.date.Date > DateTime.Now.Date ? "future" : "today";
            typeTasks = task.status == "Выполнена" ? "completed" : typeTasks;
            var taskOut = new TasksOut
            {
                id = task.id,
                code = task.code,
                desc = task.desc,
                TaskCodeParent = task.TaskCodeParent,
                projectCode = task.projectCode,
                projectId = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).id,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.supervisor).id,
                recipientId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.recipient).id,
                supervisor = task.supervisor,
                recipient = task.recipient,
                priority = task.priority,
                comment = task.comment,
                plannedTime = task.plannedTime.ToString(@"hh\:mm"),
                actualTime = task.actualTime.ToString(@"hh\:mm"),
                start = task.start,
                finish = task.finish,
                date = task.date.ToString(@"yyyy\-MM\-dd"),
                Stage = task.Stage,
                liteTask = task.liteTask,
                status = task.status,
                startWork = task.startWork,
                creator = task.creator,
                historyWorc = task.historyWorc,
                dedline = task.dedline,
                creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id

            };
            var outt = new
            {
                message = "Статус успешно обновлен!",
                value = taskOut,
                type = typeTasks
            };
            return new JsonResult(new ObjectResult(outt) { StatusCode = 202 });
        }


        [Authorize]
        [HttpGet]
        public JsonResult GetSearch([FromQuery] string param)// поиск по описанию задачи - возвраащет список задач в которых описание содержит передаваемый текст
        {
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
            }
            List<TasksOut> result = new List<TasksOut>();

            List<Tasks> tasks = _appDB.DBTask.Where(p => p.desc.Contains(param)).ToList();

            foreach (var task in tasks)
            {
                try
                {
                    var outt = new TasksOut
                    {
                        id = task.id,
                        code = task.code,
                        desc = task.desc,
                        TaskCodeParent = task.TaskCodeParent,
                        projectId = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).id,
                        supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.supervisor).id,
                        recipientId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.recipient).id,
                        projectCode = task.projectCode,
                        supervisor = task.supervisor,
                        recipient = task.recipient,
                        priority = task.priority,
                        comment = task.comment,
                        plannedTime = task.plannedTime.ToString(@"hh\:mm"),
                        actualTime = task.actualTime.ToString(@"hh\:mm"),
                        start = task.start,
                        finish = task.finish,
                        date = task.date.ToString(@"yyyy\-MM\-dd"),
                        Stage = task.Stage,
                        liteTask = task.liteTask,
                        status = task.status,
                        startWork = task.startWork,
                        creator = task.creator,
                        historyWorc = task.historyWorc,
                        dedline = task.dedline,
                        creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id

                    };
                    result.Add(outt);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (result != null) return new JsonResult(new ObjectResult(result) { StatusCode = 200 });
            else return new JsonResult(new ObjectResult("no matches!") { StatusCode = 204 });
        }

        ////////////// PROJECT
        [Authorize]
        [HttpGet]
        public JsonResult GetProjects([FromQuery] ProjectParameters ProjParam)// список проектов + 2 фильтра: в архиве или текущие, все гипы или выборочно, если есть id - выдает инф по проекту
        {
            // проверка сесии
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
            }

            if (ProjParam.id != -1)
            {
                // возвращает инфу по id проекта
                ProjectOut outt = new ProjectOut();
                try
                {
                    var project = _project.GetProject(ProjParam.id);
                    outt = new ProjectOut()
                    {
                        id = project.id,
                        code = project.code,
                        name = project.name,
                        shortName = project.shortName,
                        priority = project.priority,
                        dateStart = project.dateStart,
                        plannedFinishDate = project.plannedFinishDate,
                        actualFinishDate = project.actualFinishDate,
                        supervisor = project.supervisor,
                        supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == project.supervisor).id,
                        link = project.link,
                        history = project.history,
                        archive = project.archive,
                        nowStage = project.nowStage,
                        allStages = project.allStages,
                        timeWork = project.timeWork.ToString(@"hh\:mm")
                    };
                }
                catch (Exception)
                {
                    return new JsonResult(new ObjectResult($"Проект c id {ProjParam.id} - не найден!") { StatusCode = 404 });
                }
                return new JsonResult(new ObjectResult(outt) { StatusCode = 200 });
            }
            if (ProjParam.filterGip == "" && ProjParam.filterProj == "" && ProjParam.filterResipirnt == "")
            {
                var projects = _project.AllProjects;

                //задачи на сегодня

                TasksTableReturnModels outputTask = _task.GetMoreTasks(null, roleSession);

                var proj = projects.ToList();
                List<ProjectOut> ProjOut = new List<ProjectOut>();
                foreach (var project in proj)
                {
                    try
                    {
                        ProjectOut outt = new ProjectOut()
                        {
                            id = project.id,
                            code = project.code,
                            name = project.name,
                            shortName = project.shortName,
                            priority = project.priority,
                            dateStart = project.dateStart,
                            plannedFinishDate = project.plannedFinishDate,
                            actualFinishDate = project.actualFinishDate,
                            supervisor = project.supervisor,
                            supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == project.supervisor).id,
                            link = project.link,
                            history = project.history,
                            archive = project.archive,
                            nowStage = project.nowStage,
                            allStages = project.allStages,
                            timeWork = project.timeWork.ToString(@"hh\:mm")
                        };
                        ProjOut.Add(outt);
                    }
                    catch (Exception)
                    {
                        ProjectOut outt = new ProjectOut()
                        {
                            id = project.id,
                            code = project.code,
                            name = project.name,
                            shortName = project.shortName,
                            priority = project.priority,
                            dateStart = project.dateStart,
                            plannedFinishDate = project.plannedFinishDate,
                            actualFinishDate = project.actualFinishDate,
                            supervisor = project.supervisor,
                            supervisorId = -1,
                            link = project.link,
                            history = project.history,
                            archive = project.archive,
                            nowStage = project.nowStage,
                            allStages = project.allStages,
                            timeWork = project.timeWork.ToString(@"hh\:mm")
                        };
                        ProjOut.Add(outt);
                    }
                }

                
                var filterGipContent = new List<string>();
                foreach (var stafs in _appDB.DBStaff.Where(p => p.roleCod == "R02").ToList())
                {
                    filterGipContent.Add(stafs.name);
                }

                var filterResipirntContent = new List<string>();
                var allTasks = outputTask.today; allTasks.AddRange(outputTask.completed); allTasks.AddRange(outputTask.future);

                foreach (var task in allTasks.OrderBy(p => p.supervisor))
                {
                    if (!filterResipirntContent.Contains(task.supervisor)) filterResipirntContent.Add(task.supervisor);
                }
                
                ProjectTableReturnModelsNull output = new ProjectTableReturnModelsNull
                {
                    // проекты
                    projects = ProjOut,
                    // задачи на чегодня
                    today = outputTask.today,

                    // выполненные задачи
                    completed = outputTask.completed,

                    // будущие задачи 
                    future = outputTask.future,
                    filters = new
                    {
                        filterGip = filterGipContent,
                        filterProj = new List<string>() { "Текущие проекты", "Проекты в архиве" },
                        filterResipirnt = filterResipirntContent
                    }
                };

                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }
            else
            {
                var projects = _project.AllProjects.ToList();

                List<Project> ProjTable = new List<Project>();
                // фильтрация архивных проектов
                foreach (var filter in ProjParam.filterProj.Split(','))
                {
                    if (filter == "Проекты в архиве")
                    {
                        ProjTable.AddRange(projects.Where(p => p.archive == "Да").ToList());
                    }
                    if (filter == "Текущие проекты")
                    {
                        ProjTable.AddRange(projects.Where(p => p.archive == "Нет").ToList());
                    }
                }

                // фильтрация по ответсвенному за проекты
                foreach (var filter in ProjParam.filterGip.Split(','))
                {
                    if (filter != "Все ГИПы" && filter != "")
                    {
                        ProjTable.AddRange(projects.Where(p => p.supervisor == filter).ToList());
                    }
                }

                if (ProjTable.Count() == 0) ProjTable = projects;
                List<string> projNames = new List<string>();
                foreach (var project in ProjTable)
                {
                    if (!projNames.Contains(project.code)) projNames.Add(project.code);
                }

                List<string> staffNames = new List<string>();
                staffNames.Add(roleSession.SessionName);
                foreach (var task in _staff.StaffTable(roleSession.SessionRole, sessionCod))
                {
                    if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
                }

                TasksTableReturnModels outputTask = _task.GetMoreTasks(staffNames, roleSession, "", projNames);

                
                List<TasksOut> todays = outputTask.today;
                List<TasksOut> completeds = outputTask.completed;
                List<TasksOut> futures = outputTask.future;
                foreach (var filter in ProjParam.filterResipirnt.Split(','))
                {
                    if (filter != "Все ответственные" && filter != "")
                    {
                        todays = todays.Where(p => p.recipient == filter || (p.recipient == null && p.supervisor == filter)).ToList();
                        completeds = completeds.Where(p => p.recipient == filter || (p.recipient == null && p.supervisor == filter)).ToList();
                        futures = futures.Where(p => p.recipient == filter || (p.recipient == null && p.supervisor == filter)).ToList();
                    }
                }

                

                var proj = ProjTable.ToList();
                List<ProjectOut> ProjOut = new List<ProjectOut>();
                foreach (var project in proj)
                {
                    try
                    {
                        ProjectOut outt = new ProjectOut()
                        {
                            id = project.id,
                            code = project.code,
                            name = project.name,
                            shortName = project.shortName,
                            priority = project.priority,
                            dateStart = project.dateStart,
                            plannedFinishDate = project.plannedFinishDate,
                            actualFinishDate = project.actualFinishDate,
                            supervisor = project.supervisor,
                            supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == project.supervisor).id,
                            link = project.link,
                            history = project.history,
                            archive = project.archive,
                            nowStage = project.nowStage,
                            allStages = project.allStages,
                            timeWork = project.timeWork.ToString(@"hh\:mm")
                        };
                        ProjOut.Add(outt);
                    }
                    catch (Exception)
                    {
                        ProjectOut outt = new ProjectOut()
                        {
                            id = project.id,
                            code = project.code,
                            name = project.name,
                            shortName = project.shortName,
                            priority = project.priority,
                            dateStart = project.dateStart,
                            plannedFinishDate = project.plannedFinishDate,
                            actualFinishDate = project.actualFinishDate,
                            supervisor = project.supervisor,
                            supervisorId = -1,
                            link = project.link,
                            history = project.history,
                            archive = project.archive,
                            nowStage = project.nowStage,
                            allStages = project.allStages,
                            timeWork = project.timeWork.ToString(@"hh\:mm")
                        };
                        ProjOut.Add(outt);
                    }


                }

                var filterGipContent = new List<string>();
                foreach (var stafs in _appDB.DBStaff.Where(p => p.roleCod == "R02").ToList())
                {
                    filterGipContent.Add(stafs.name);
                }

                var filterResipirntContent = new List<string>();
                var allTasks = todays; allTasks.AddRange(completeds); allTasks.AddRange(futures);

                foreach (var task in allTasks.OrderBy(p => p.supervisor))
                {
                    if (!filterResipirntContent.Contains(task.supervisor)) filterResipirntContent.Add(task.supervisor);
                }

                ProjectTableReturnModelsNull output = new ProjectTableReturnModelsNull
                {
                    // проекты
                    projects = ProjOut,
                    // задачи на чегодня
                    today = todays,

                    // выполненные задачи
                    completed = completeds,

                    // будущие задачи 
                    future = futures,
                    filters = new
                    {
                        filterGip = filterGipContent,
                        filterProj = new List<string>() { "Текущие проекты", "Проекты в архиве" },
                        filterResipirnt = filterResipirntContent
                    }
                };

                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }
        }

         
        [Authorize]
        [HttpPut]
        public JsonResult PutProj([FromBody] ProjectParameters ProjParam)// обновляет проект
        {
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
            }

            if (ProjParam.id == -1 ||
                ProjParam.code.Length < 1 ||
                ProjParam.shortName.Length < 1 ||
                ProjParam.arhive.Length < 1)
            {
                string contentError = "";
                if (ProjParam.id == -1) contentError += "Id проекта; ";
                if (ProjParam.code.Length < 1) contentError += "Шифр; ";
                if (ProjParam.shortName.Length < 1) contentError += "Краткое название; ";
                if (ProjParam.arhive.Length < 1) contentError += "Архив (Да или Нет); ";

                var error = new
                {
                    messsage = "Не все поля заполнены!",
                    content = "Не заполнены поля: " + contentError
                };
                return new JsonResult(new ObjectResult(error) { StatusCode = 400 });
            }

            string supervisor = null;
            try
            {
                supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == ProjParam.supervisor).name;
            }
            catch (Exception) { return new JsonResult(new ObjectResult("Указанный ответственный не найден!") { StatusCode = 404 }); }

            _project.redactToDB(ProjParam.id, ProjParam.code, ProjParam.shortName, ProjParam.name, ProjParam.arhive, ProjParam.link, supervisor, ProjParam.priority, ProjParam.allStages);
            LogisticProject item = new LogisticProject()
            {
                arhive = ProjParam.arhive,
                projectId = ProjParam.id,
                link = ProjParam.link,
                supervisor = supervisor,
                priority = ProjParam.priority,
                allStages = ProjParam.allStages,
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                dateRedaction = DateTime.Now.AddHours(-5),
                comment = ProjParam.comment.Length > 1 ? ProjParam.comment : null
            };

            _logistickProject.addToDB(item);
            var outt = new
            {
                message = "Проект успешно обновлен!",
                value = _appDB.DBProject.FirstOrDefault(p => p.id == ProjParam.id)
            };
            return new JsonResult(new ObjectResult(outt) { StatusCode = 202 });
        }


        [Authorize]
        [HttpPost]
        public JsonResult PostProj([FromBody] ProjectParameters ProjParam)//добавляет проект в базу
        {
            try
            {
                var proj = _appDB.DBProject.FirstOrDefault(p => p.code == ProjParam.code);
                return new JsonResult(new ObjectResult($"Проекст {proj.code} - уже существует!") { StatusCode = 400 });
            }
            catch (Exception)
            {

            }
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
            }

            if (ProjParam.code.Length < 1 ||
               ProjParam.shortName.Length < 1 ||
               ProjParam.priority == -2)
            {
                string contentError = "";
                if (ProjParam.code.Length < 1) contentError += "Шифр; ";
                if (ProjParam.shortName.Length < 1) contentError += "Краткое название; ";
                if (ProjParam.priority == -2) contentError += "Приоритет; ";

                var error = new
                {
                    messsage = "Не все поля заполнены!",
                    content = "Не заполнены поля: " + contentError
                };
                return new JsonResult(new ObjectResult(error) { StatusCode = 400 });
            }

            string supervisor = null;
            try
            {
                supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == ProjParam.supervisor).name;
            }
            catch (Exception) { return new JsonResult(new ObjectResult("Указанный ответственный не найден!") { StatusCode = 404 }); }

            var plannedFinishDate = new DateTime();
            try
            {
                plannedFinishDate = DateTime.Parse(ProjParam.plannedFinishDate);
            }
            catch (Exception) { return new JsonResult(new ObjectResult("Неверный формат даты!") { StatusCode = 400 }); }

            var item = new Project
            {
                code = ProjParam.code,
                name = ProjParam.name,
                shortName = ProjParam.shortName,
                priority = ProjParam.priority,
                dateStart = DateTime.Now,
                plannedFinishDate = plannedFinishDate,
                supervisor = supervisor,
                link = ProjParam.link,
                archive = "Нет",
                nowStage = ProjParam.allStages == null ? "" : ProjParam.allStages.Split(',')[0],
                allStages = ProjParam.allStages,
                history = $"{DateTime.Now.AddHours(-5)} - Проект создан"
            };

            LogisticProject log = new LogisticProject()
            {
                arhive = "Нет",
                projectId = item.id,
                link = ProjParam.link,
                supervisor = supervisor,
                priority = ProjParam.priority,
                allStages = ProjParam.allStages,
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                dateRedaction = DateTime.Now.AddHours(-5),
                comment = "Проект создан"
            };

            _logistickProject.addToDB(log);
            _project.addToDB(item);

            var outt = new
            {
                message = "Проект успешно добавлен!",
                value = item
            };

            return new JsonResult(new ObjectResult(outt) { StatusCode = 201 });
        }


        //////////// STAFF
        [Authorize]
        [HttpGet]
        public JsonResult GetEmployees([FromQuery] StaffParameters StaffParam)// список сотрудников
        {
            // проверка сессии - без входа в сессию нужно переходить на траницу авторизации
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                var person = _appDB.DBStaff.FirstOrDefault(p => p.login == User.Identity.Name);

                var post = person.post;
                var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
                roleSession = new SessionRoles()
                {
                    SessionName = person.name,
                    SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name
                };
                sessionCod = person.code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 401 });
                
            }

            // списка сотрудников без фильтрации
            if (StaffParam.filterPosts == "" && StaffParam.filterTasks == "" && StaffParam.filterStaffs == "")
            {
                
                TasksTableReturnModels tasksTabbleFilter = _task.GetMoreTasks(null, roleSession, StaffParam.filterTasks);

                var staffs = _staff.StaffTable(roleSession.SessionRole, sessionCod).ToList();
                var filterPosts = new List<string>();
                foreach (var staf in staffs)
                {
                    if (!filterPosts.Contains(staf.post)) filterPosts.Add(staf.post);
                }
                var filterStaffs = new List<string>();
                foreach (var staf in staffs)
                {
                    if (!filterStaffs.Contains(staf.name)) filterStaffs.Add(staf.name);
                }

                StaffTableReturnModelsNull output = new StaffTableReturnModelsNull
                {
                    // список сотрудников
                    staffs = staffs,
                    // задачи на чегодня
                    today = tasksTabbleFilter.today,
                    // выполненные задачи
                    completed = tasksTabbleFilter.completed,
                    // будущие задачи 
                    future = tasksTabbleFilter.future,

                    filters = new
                    {
                        filterTasks = new List<string>() { "Мои задачи", "Все задачи" },
                        filterPosts = filterPosts,
                        filterStaffs = filterStaffs
                    }
                };

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }
            // список сотрудников с фильтром по должности
            else
            {
                var staffs = _staff.StaffTable(roleSession.SessionRole, sessionCod).ToList();
                var StaffTable = new List<StaffOut>();

                foreach (var filter in StaffParam.filterPosts.Split(','))
                {
                    if (filter != "" && filter != "Все должности")
                    {
                        try
                        {
                            StaffTable.AddRange(staffs.Where(p => p.post == filter).ToList());
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                foreach (var filter in StaffParam.filterStaffs.Split(','))
                {
                    if (filter != "" && filter != "Все сотрудники")
                    {
                        try
                        {
                            StaffTable.AddRange(staffs.Where(p => p.name == filter).ToList());
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }

                if (StaffTable.Count() == 0) StaffTable = staffs;
                // составление списка сотрудников в подчинениии у того кто вошел в сессию
                List<string> staffNames = new List<string>();
                foreach (var task in StaffTable)
                {
                    if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
                }

                // список задач сотрудников из вышеупомянутого списка
                TasksTableReturnModels tasksTabbleFilter = _task.GetMoreTasks(staffNames, roleSession, StaffParam.filterTasks);


                var filterPosts = new List<string>();
                foreach (var staf in staffs)
                {
                    if (!filterPosts.Contains(staf.post)) filterPosts.Add(staf.post);
                }
                var filterStaffs = new List<string>();
                foreach (var staf in staffs)
                {
                    if (!filterStaffs.Contains(staf.name)) filterStaffs.Add(staf.name);
                }

                StaffTableReturnModelsNull output = new StaffTableReturnModelsNull
                {
                    // список сотрудников
                    staffs = staffs,
                    // задачи на чегодня
                    today = tasksTabbleFilter.today,
                    // выполненные задачи
                    completed = tasksTabbleFilter.completed,
                    // будущие задачи 
                    future = tasksTabbleFilter.future,

                    filters = new
                    {
                        filterTasks = new List<string>() { "Мои задачи", "Все задачи" },
                        filterPosts = filterPosts,
                        filterStaffs = filterStaffs
                    }
                };

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }
        }

        [Authorize]
        [HttpGet]
        public JsonResult GetPosts()// список должностей
        {
            var posts = _post.AllPosts.ToList();
            return new JsonResult(new ObjectResult(posts) { StatusCode = 200 });
        }


        [Authorize]
        [HttpGet]
        public JsonResult Getguide()// возвращает структура справочника
        {
            List<GuideStaff> managDep = new List<GuideStaff>();
            var ennum = _appDB.DBStaff.Where(p => p.divisionId == 1).OrderBy(p => p.roleCod).ToList();
            foreach (var staff in ennum)
            {
                managDep.Add(
                    new GuideStaff
                    {
                        name = staff.name,
                        post = staff.post,
                        role = _appDB.DBRole.FirstOrDefault(p => p.code == staff.roleCod).name
                    });
            }


            List<GuideStaff> disDep = new List<GuideStaff>();
            var staffNO = _appDB.DBStaff.FirstOrDefault(p => p.code == "07");
            disDep.Add(
                   new GuideStaff
                   {
                       name = staffNO.name,
                       post = staffNO.post,
                       role = _appDB.DBRole.FirstOrDefault(p => p.code == staffNO.roleCod).name
                   });
            ennum = _appDB.DBStaff.Where(p => p.divisionId == 2).ToList();
            foreach (var staff in ennum.Where(p => p.supervisorCod == "07" && p.roleCod == "R06"))
            {
                disDep.Add(
                    new GuideStaff
                    {
                        name = staff.name,
                        post = staff.post,
                        role = _appDB.DBRole.FirstOrDefault(p => p.code == staff.roleCod).name
                    });
            }
            foreach (var staffRG in ennum.Where(p => p.supervisorCod == "07" && p.roleCod == "R05"))
            {
                disDep.Add(
                    new GuideStaff
                    {
                        name = staffRG.name,
                        post = staffRG.post,
                        role = _appDB.DBRole.FirstOrDefault(p => p.code == staffRG.roleCod).name
                    });
                foreach (var staff in ennum.Where(p => p.supervisorCod == staffRG.code))
                    disDep.Add(
                        new GuideStaff
                        {
                            name = staff.name,
                            post = staff.post,
                            role = _appDB.DBRole.FirstOrDefault(p => p.code == staff.roleCod).name
                        });
            }


            List<GuideStaff> researchDep = new List<GuideStaff>();
            staffNO = _appDB.DBStaff.FirstOrDefault(p => p.code == "27");
            disDep.Add(
                   new GuideStaff
                   {
                       name = staffNO.name,
                       post = staffNO.post,
                       role = _appDB.DBRole.FirstOrDefault(p => p.code == staffNO.roleCod).name
                   });
            ennum = _appDB.DBStaff.Where(p => p.divisionId == 3).ToList();
            foreach (var staff in ennum.Where(p => p.supervisorCod == "27" && p.roleCod == "R06"))
            {
                disDep.Add(
                    new GuideStaff
                    {
                        name = staff.name,
                        post = staff.post,
                        role = _appDB.DBRole.FirstOrDefault(p => p.code == staff.roleCod).name
                    });
            }
            foreach (var staffRG in ennum.Where(p => p.supervisorCod == "27" && p.roleCod == "R05"))
            {
                disDep.Add(
                    new GuideStaff
                    {
                        name = staffRG.name,
                        post = staffRG.post,
                        role = _appDB.DBRole.FirstOrDefault(p => p.code == staffRG.roleCod).name
                    });
                foreach (var staff in ennum.Where(p => p.supervisorCod == staffRG.code))
                    disDep.Add(
                        new GuideStaff
                        {
                            name = staff.name,
                            post = staff.post,
                            role = _appDB.DBRole.FirstOrDefault(p => p.code == staff.roleCod).name


                        });
            }


            List < GuideRole> guideRoles = new List<GuideRole>();
            foreach (var content in _appDB.DBRole.OrderBy(p => p.code))
            {
                guideRoles.Add(
                    new GuideRole
                    {
                        role = content.name,
                        supervisor = content.supervisor,
                        resipient = content.recipient
                    });
            }

            var outt = new
            {
                managementDepartment = new
                {
                    name = "Отдел управления",
                    array = managDep
                },
                designDepartment = new
                {
                    name = "Отдел проектирования",
                    array = disDep
                },
                researchDepartment = new
                {
                    name = "Отдел изысканий",
                    array = researchDep,
                },
                companyRoleStruct = guideRoles
            };
            return new JsonResult(new ObjectResult(outt) { StatusCode = 202 });
        }
    }
}
