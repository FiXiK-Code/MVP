using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVP.ApiModels;
using MVP.Date;
using MVP.Date.API;
using MVP.Date.Interfaces;
using MVP.Date.Models;
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

            var post = personbuf.post;
            var roleCod = _appDB.DBPost.FirstOrDefault(p => p.name == post).roleCod;
            var role = _appDB.DBRole.FirstOrDefault(p => p.code == roleCod).name;


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
        // функция проверки
        private ClaimsIdentity GetIdentity(string userName, string password)
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

                        //new Claim(ClaimsIdentity.DefaultNameClaimType, _appDB.DBStaff.FirstOrDefault(p => p.name == userName).login),
                    };
                }
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                   ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
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
                               task.supervisor, task.recipient, task.priority, task.plannedTime, task.start, task.finish, "");

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

                if (result != null) return new JsonResult(new ObjectResult("task not found") { StatusCode = 404 });
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

                // список задач сотрудников из вышеупомянутого списка


                // сборка модели для возвращения
                TasksTableReturnModels output = _task.GetMoreTasks(staffNames, roleSession, TaskParam.filterTable, true);

                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult PostTasks
            ([FromBody] TasksParameters TaskParam)//добавляет задачу в базу
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
                //return new JsonResult("Не авторизованный запрос!");////////////////
            }

            // корректировка даты - автоперенос при заполненном дне
            var supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.supervisor).name;
            var recipient = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.recipient).name;
            var date = new DateTime(Convert.ToInt32(TaskParam.date.Split('-')[0]), Convert.ToInt32(TaskParam.date.Split('-')[1]), Convert.ToInt32(TaskParam.date.Split('-')[2]));
            string projectCode = null;
            try
            {
                projectCode = _appDB.DBProject.FirstOrDefault(p => p.id == TaskParam.projectCode).code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult($"Проект c id {TaskParam.projectCode} - не найден!") { StatusCode = 404 });
            }
            var plannedTime = new TimeSpan(Convert.ToInt32(TaskParam.plannedTime.Split(':')[0]), Convert.ToInt32(TaskParam.plannedTime.Split(':')[1]), Convert.ToInt32(TaskParam.plannedTime.Split(':')[2]));
            int sec = TaskParam.dedline.Split('T')[1].Length > 5 ? Convert.ToInt32(TaskParam.dedline.Split('T')[1].Split(':')[2].Split('.')[0]) : 0;
            var dedline = new DateTime(Convert.ToInt32(TaskParam.dedline.Split('T')[0].Split('-')[0]), Convert.ToInt32(TaskParam.dedline.Split('T')[0].Split('-')[1]), Convert.ToInt32(TaskParam.dedline.Split('T')[0].Split('-')[2]),
                Convert.ToInt32(TaskParam.dedline.Split('T')[1].Split(':')[0]), Convert.ToInt32(TaskParam.dedline.Split('T')[1].Split(':')[1]),sec);
            date = redackPriorAndPerenos(supervisor, date, plannedTime, projectCode, TaskParam.liteTask);

            // добавление задачи в базу
            var item = new Tasks
            {
                actualTime = TimeSpan.Zero,
                desc = TaskParam.desc,
                projectCode = projectCode,
                supervisor = supervisor,
                recipient = recipient,
                priority = TaskParam.liteTask == "Задача" ? _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).priority : -1,
                comment = TaskParam.comment != null ? $"{roleSession.SessionName}: {TaskParam.comment}\n" : null,
                plannedTime = plannedTime,
                date = date,
                dedline = dedline,
                Stage = TaskParam.Stage,
                status = "Создана",
                liteTask = TaskParam.liteTask == "Задача" ? false : true,
                creator = roleSession.SessionName

            };
            _task.addToDB(item);

            // заполнение лога
            var task = _appDB.DBTask.FirstOrDefault(p => p.desc == TaskParam.desc);
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
                comment = "Задача создана. Комментарий: " + TaskParam.comment
            };
            _logistickTask.addToDB(log);


            return new JsonResult(new ObjectResult("Задача добавлена!") { StatusCode = 201 });
        }

        [Authorize]
        [HttpPut]
        public JsonResult PutTasks
            ([FromBody] TasksParameters TaskParam)// обновляет задачу
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

            if (result != null) return new JsonResult(new ObjectResult("task not found") { StatusCode = 404 });

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
                //return new JsonResult("Не авторизованный запрос!");////////////////
            }
            var supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.supervisor).name;
            var recipient = _appDB.DBStaff.FirstOrDefault(p => p.id == TaskParam.recipient).name;
            var date = new DateTime(Convert.ToInt32(TaskParam.date.Split('-')[0]), Convert.ToInt32(TaskParam.date.Split('-')[1]), Convert.ToInt32(TaskParam.date.Split('-')[2]));
            string projectCode = null;
            try
            {
                projectCode = _appDB.DBProject.FirstOrDefault(p => p.id == TaskParam.projectCode).code;
            }
            catch (Exception)
            {
                return new JsonResult(new ObjectResult($"Проект c id {TaskParam.projectCode} - не найден!") { StatusCode = 404 });
            }
            var plannedTime = new TimeSpan(Convert.ToInt32(TaskParam.plannedTime.Split(':')[0]), Convert.ToInt32(TaskParam.plannedTime.Split(':')[1]), Convert.ToInt32(TaskParam.plannedTime.Split(':')[2]));
            int sec = TaskParam.start.Split('T')[1].Length > 5 ? Convert.ToInt32(TaskParam.start.Split('T')[1].Split(':')[2].Split('.')[0]) : 0;
            var start = new DateTime(Convert.ToInt32(TaskParam.start.Split('T')[0].Split('-')[0]), Convert.ToInt32(TaskParam.start.Split('T')[0].Split('-')[1]), Convert.ToInt32(TaskParam.start.Split('T')[0].Split('-')[2]),
                Convert.ToInt32(TaskParam.start.Split('T')[1].Split(':')[0]), Convert.ToInt32(TaskParam.start.Split('T')[1].Split(':')[1]), sec);
            sec = TaskParam.finish.Split('T')[1].Length > 5 ? Convert.ToInt32(TaskParam.finish.Split('T')[1].Split(':')[2].Split('.')[0]) : 0;
            var finish = new DateTime(Convert.ToInt32(TaskParam.finish.Split('T')[0].Split('-')[0]), Convert.ToInt32(TaskParam.finish.Split('T')[0].Split('-')[1]), Convert.ToInt32(TaskParam.finish.Split('T')[0].Split('-')[2]),
                Convert.ToInt32(TaskParam.finish.Split('T')[1].Split(':')[0]), Convert.ToInt32(TaskParam.finish.Split('T')[1].Split(':')[1]), sec);
            sec = TaskParam.dedline.Split('T')[1].Length > 5 ? Convert.ToInt32(TaskParam.dedline.Split('T')[1].Split(':')[2].Split('.')[0]) : 0;
            var dedline = new DateTime(Convert.ToInt32(TaskParam.dedline.Split('T')[0].Split('-')[0]), Convert.ToInt32(TaskParam.dedline.Split('T')[0].Split('-')[1]), Convert.ToInt32(TaskParam.dedline.Split(' ')[0].Split('-')[2]),
                Convert.ToInt32(TaskParam.dedline.Split('T')[1].Split(':')[0]), Convert.ToInt32(TaskParam.dedline.Split('T')[1].Split(':')[1]), sec);
            
            // корректировка даты - автоперенос при заполненном дне
            date = redackPriorAndPerenos(supervisor, date, plannedTime, _appDB.DBTask.FirstOrDefault(p => p.id == TaskParam.id).projectCode, TaskParam.liteTask);

            // попытка редактирования задачи
            if (!_task.redactToDB(TaskParam.liteTask, TaskParam.id, date, dedline, TaskParam.status, TaskParam.comment != null ? $"{roleSession.SessionName}: {TaskParam.comment}\n" : null, supervisor, recipient, TaskParam.pririty, plannedTime, start, finish, roleSession.SessionName))
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
                    comment = TaskParam.comment
                };
                _logistickTask.addToDB(item);


                return new JsonResult(new ObjectResult("Задача успешно обновлена!") { StatusCode = 202 });
            }
        }


        [Authorize]
        [HttpPut]
        public JsonResult PutTasksStatus
            ([FromBody] TasksParameters TaskParam)// обновляет статус задачи
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
                //return new JsonResult("Не авторизованный запрос!");////////////////
            }
            //if (TaskParam.status == "В работе") _task.timeWork(TaskParam.id, _task);
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

            return new JsonResult(new ObjectResult("Статус успешно обновлен!") { StatusCode = 202 });
        }
        //////// ????
        //[Authorize]
        //[HttpDelete]
        //public JsonResult DeleteTasks()// удаляет задачу???
        //{
        //    return new JsonResult("");
        //}

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
                //return new JsonResult("Не авторизованный запрос!");////////////////
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
                        date = task.date.ToString(@"dd\.MM\.yyyy"),
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

        ////////// projects
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
                //return new JsonResult("Не авторизованный запрос!");////////////////
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

                var today = _task.AllTasks.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList();

                List<TasksOut> todayOut = new List<TasksOut>();
                foreach (var task in today)
                {
                    try
                    {
                        var outt = new TasksOut()
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
                            date = task.date.ToString(@"dd\.MM\.yyyy"),
                            Stage = task.Stage,
                            liteTask = task.liteTask,
                            status = task.status,
                            startWork = task.startWork,
                            creator = task.creator,
                            historyWorc = task.historyWorc,
                            dedline = task.dedline,
                            creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id

                        };
                        todayOut.Add(outt);
                    }
                    catch (Exception)
                    {
                        continue;
                    }


                }



                // выполненные задачи
                var completed = _task.AllTasks.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList();

                List<TasksOut> completedOut = new List<TasksOut>();
                foreach (var task in completed)
                {
                    try
                    {
                        var outt = new TasksOut()
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
                            date = task.date.ToString(@"dd\.MM\.yyyy"),
                            Stage = task.Stage,
                            liteTask = task.liteTask,
                            status = task.status,
                            startWork = task.startWork,
                            creator = task.creator,
                            historyWorc = task.historyWorc,
                            dedline = task.dedline,
                            creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id

                        };
                        completedOut.Add(outt);
                    }
                    catch (Exception)
                    {
                        continue;
                    }


                }
                // будущие задачи 
                var future = _task.AllTasks.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList();

                List<TasksOut> futureOut = new List<TasksOut>();
                foreach (var task in future)
                {
                    try
                    {
                        TasksOut outt = new TasksOut()
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
                            date = task.date.ToString(@"dd\.MM\.yyyy"),
                            Stage = task.Stage,
                            liteTask = task.liteTask,
                            status = task.status,
                            startWork = task.startWork,
                            creator = task.creator,
                            historyWorc = task.historyWorc,
                            dedline = task.dedline,
                            creatorId = _appDB.DBStaff.FirstOrDefault(p => p.name == task.creator).id
                        };
                        futureOut.Add(outt);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }

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


                ProjectTableReturnModels output = new ProjectTableReturnModels
                {
                    // проекты
                    projects = ProjOut,
                    // задачи на чегодня
                    today = todayOut,

                    // выполненные задачи
                    completed = completedOut,

                    // будущие задачи 
                    future = futureOut

                };

                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }
        }

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
                //return new JsonResult("Не авторизованный запрос!");////////////////
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

                TasksTableReturnModels tasksTabbleFilter = _task.GetMoreTasks(staffNames, roleSession, "");

                StaffTableReturnModels output = new StaffTableReturnModels
                {
                    // список сотрудников
                    staffs = _staff.StaffTable(roleSession.SessionRole, sessionCod).ToList(),
                    // задачи на чегодня
                    today = tasksTabbleFilter.today,
                    // выполненные задачи
                    completed = tasksTabbleFilter.completed,
                    // будущие задачи 
                    future = tasksTabbleFilter.future
                };

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
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
                TasksTableReturnModels tasksTabbleFilter = _task.GetMoreTasks(staffNames, roleSession, "", true);

                StaffTableReturnModels output = new StaffTableReturnModels
                {
                    staffs = StaffTable,
                    // задачи на чегодня
                    today = tasksTabbleFilter.today,
                    // выполненные задачи
                    completed = tasksTabbleFilter.completed,
                    // будущие задачи 
                    future = tasksTabbleFilter.future
                };

                // возвращает список сотрудников в подчинении у залогиненного пользователя
                return new JsonResult(new ObjectResult(output) { StatusCode = 200 });
            }
        }

        ///// project

        [Authorize]
        [HttpPut]
        public JsonResult PutProj
            ([FromBody] ProjectParameters ProjParam)// обновляет проект
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
                //return new JsonResult("Не авторизованный запрос!");////////////////
            }

            var supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == ProjParam.supervisor).name;

            _project.redactToDB(ProjParam.id, ProjParam.arhive, ProjParam.link, supervisor, ProjParam.priority, ProjParam.allStages);
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
                comment = ProjParam.comment
            };

            _logistickProject.addToDB(item);

            return new JsonResult(new ObjectResult("Проект успешно обновлен!") { StatusCode = 202 });
        }

        [Authorize]
        [HttpPost]
        public JsonResult PostProj
            ([FromBody] ProjectParameters ProjParam)//добавляет проект в базу
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
                return new JsonResult(new ObjectResult("Не авторизованный запрос!") { StatusCode = 400 });
                //return new JsonResult("Не авторизованный запрос!");////////////////
            }

            var supervisor = _appDB.DBStaff.FirstOrDefault(p => p.id == ProjParam.supervisor).name;
            int sec = ProjParam.plannedFinishDate.Split('T')[1].Length > 5 ? Convert.ToInt32(ProjParam.plannedFinishDate.Split('T')[1].Split(':')[2].Split('.')[0]) : 0;
            var plannedFinishDate = new DateTime(Convert.ToInt32(ProjParam.plannedFinishDate.Split('T')[0].Split('-')[0]), Convert.ToInt32(ProjParam.plannedFinishDate.Split('T')[0].Split('-')[1]), Convert.ToInt32(ProjParam.plannedFinishDate.Split('T')[0].Split('-')[2]),
                Convert.ToInt32(ProjParam.plannedFinishDate.Split('T')[1].Split(':')[0]), Convert.ToInt32(ProjParam.plannedFinishDate.Split('T')[1].Split(':')[1]),
                sec);


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

            return new JsonResult(new ObjectResult("Проект успешно добавлен!") { StatusCode = 201 });
        }


        [Authorize]
        [HttpGet]
        public JsonResult GetPosts()// список сотрудников
        {
            var posts = _post.AllPosts.ToList();
            return new JsonResult(new ObjectResult(posts) { StatusCode = 200 });
        }

}
}
