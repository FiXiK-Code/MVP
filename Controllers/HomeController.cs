using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MVP.Date;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using MVP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = MVP.Date.Models.TaskStatus;

namespace MVP.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDB _appDB;
        private readonly IPost _post;
        private readonly IRole _role;
        private readonly ITask _task;
        private readonly IProject _project;
        private readonly ILogistickTask _logistickTask;
        private readonly ILogisticProject _logistickProject;
        private readonly IStaff _staff;
        private readonly IEmailService _emailService;

        public HomeController(IPost post, IRole role,ITask task, IProject project, AppDB appDB, ILogistickTask logistick, IStaff staff, ILogisticProject logistickProject, IEmailService emailService)
        {
            _post = post;
            _role = role;
            _task = task;
            _project = project;
            _appDB = appDB;
            _logistickTask = logistick;
            _staff = staff;
            _logistickProject = logistickProject;
            _emailService = emailService;
        }

        public RedirectToActionResult RedactSatusTask(int id, string stat,int activTable, string staffTableFilter,
            string recipientProjectFilter,
            string supervisorProjectFilter,
            string porjectFiltr,
            string filterTaskTable,
            string filterStaffTable)
        {
            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }
            //if (stat == "В работе") _task.timeWork(id);
            if(_task.GetTask(id).recipient != roleSession.SessionName && _task.GetTask(id).recipient != null)
            {
                    var msg = "Нельзя менять статус чужих задач!";
                    return RedirectToAction("TaskTable", new
                    {
                        activTable = activTable,
                        Taskid = id,
                        meesage = msg,
                        TaskRed = true,
                        staffTableFilter = staffTableFilter,
                        recipientProjectFilter = recipientProjectFilter,
                        supervisorProjectFilter = supervisorProjectFilter,
                        porjectFiltr = porjectFiltr,
                        filterStaffTable = filterStaffTable
                    });
            }
            if (!_task.redactStatusAsync(id, stat,roleSession.SessionName).Result)
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return RedirectToAction("TaskTable", new { activTable = activTable, Taskid = id, meesage = msg, TaskRed = true,
                    staffTableFilter = staffTableFilter,
                    recipientProjectFilter = recipientProjectFilter,
                    supervisorProjectFilter = supervisorProjectFilter,
                    porjectFiltr = porjectFiltr,
                    filterStaffTable = filterStaffTable
                });
            }
            
            var supervisor = _appDB.DBTask.FirstOrDefault(p => p.id == id).supervisor;
            LogistickTask item = new LogistickTask()
            {
                ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == id).projectCode,
                TaskId = id,
                descTask = _appDB.DBTask.FirstOrDefault(p => p.id == id).desc,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                dateRedaction = DateTime.Now.AddHours(-5),
                planedTime = _appDB.DBTask.FirstOrDefault(p => p.id == id).plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == _appDB.DBTask.FirstOrDefault(p => p.id == id).status).id,
                comment = $"Стату задачи изменен на: {stat}"
            };
            _logistickTask.addToDB(item);

            return RedirectToAction("TaskTable", new { activTable = activTable,
                staffTableFilter = staffTableFilter,
                recipientProjectFilter = recipientProjectFilter,
                supervisorProjectFilter = supervisorProjectFilter,
                porjectFiltr = porjectFiltr,
                filterTaskTable = filterTaskTable,
                filterStaffTable = filterStaffTable
            });
        }

        public RedirectToActionResult RedactProjectToDB(
            int iid,
            string arhive,
            string link,
            string supervisor,
            int priority,
            string allStages,
            string comment,
            int activTable,
            string staffTableFilter,
            string recipientProjectFilter,
            string supervisorProjectFilter,
            string porjectFiltr,
            string filterTaskTable,
            string filterStaffTable
            )
        {
           

            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }
            _project.redactToDB(iid, arhive, link, supervisor, priority, allStages);
            LogisticProject item = new LogisticProject()
            {
                arhive = arhive,
                projectId = iid,
                link = link,
                supervisor = supervisor,
                priority = priority,
                allStages = allStages,
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                dateRedaction = DateTime.Now.AddHours(-5),
                comment = comment
            };

            _logistickProject.addToDB(item);
            return RedirectToAction("TaskTable", new { activTable = activTable ,Projid = iid,
                staffTableFilter = staffTableFilter,
                recipientProjectFilter = recipientProjectFilter,
                supervisorProjectFilter = supervisorProjectFilter,
                porjectFiltr = porjectFiltr,
                filterTaskTable = filterTaskTable,
                filterStaffTable = filterStaffTable
            });

        }
        public RedirectToActionResult RedactTaskToDB(
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
            int activTable,
            string staffTableFilter,
            string recipientProjectFilter,
            string supervisorProjectFilter,
            string porjectFiltr,
            string filterTaskTable,
            string filterStaffTable
)
        {
            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }

            date = redackPriorAndPerenos(supervisor, date, plannedTime, _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode, liteTask);

            if (recipient != roleSession.SessionName && recipient != null)
            {
                if (status == "В работе" && recipient == _task.GetTask(iid).recipient)
                {
                    var msg = "Нельзя менять статус чужих задач!";
                    return RedirectToAction("TaskTable", new
                    {
                        activTable = activTable,
                        Taskid = iid,
                        meesage = msg,
                        TaskRed = true,
                        staffTableFilter = staffTableFilter,
                        recipientProjectFilter = recipientProjectFilter,
                        supervisorProjectFilter = supervisorProjectFilter,
                        porjectFiltr = porjectFiltr,
                        filterStaffTable = filterStaffTable
                    });
                }
            }

            if (!_task.redactToDB(liteTask, iid, date, dedline, status, comment != null ? $"{roleSession.SessionName}: {comment}\n" : null, supervisor, recipient, pririty, plannedTime, start, finish, roleSession.SessionName))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return RedirectToAction("TaskTable", new { activTable = activTable, Taskid = iid, meesage = msg, TaskRed = true,
                    staffTableFilter = staffTableFilter,
                    recipientProjectFilter = recipientProjectFilter,
                    supervisorProjectFilter = supervisorProjectFilter,
                    porjectFiltr = porjectFiltr,
                    filterStaffTable = filterStaffTable
                });
            }
            else
            { 
                var projCod = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode;
                var projId = _appDB.DBProject.FirstOrDefault(p => p.code == projCod) != null ? _appDB.DBProject.FirstOrDefault(p => p.code == projCod).id : -1;
                _project.NextStage(projId);

                
                LogistickTask item = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                    TaskId = iid,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                    resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                    dateRedaction = DateTime.Now.AddHours(-5),
                    planedTime = plannedTime,
                    actualTime = _appDB.DBTask.FirstOrDefault(p => p.id == iid).actualTime,
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == status).id,
                    comment = comment
                };
                _logistickTask.addToDB(item);
                //if (status == "В работе") _task.timeWork(iid);
                return RedirectToAction("TaskTable", new { activTable = activTable, Taskid = iid,
                    staffTableFilter = staffTableFilter,
                    recipientProjectFilter = recipientProjectFilter,
                    supervisorProjectFilter = supervisorProjectFilter,
                    porjectFiltr = porjectFiltr,
                    filterTaskTable = filterTaskTable,
                    filterStaffTable = filterStaffTable
                });
            }

        }

        public RedirectToActionResult addProjectToDB(
            string link,
            string code,
            string supervisor,
            int priority,
             DateTime plannedFinishDate,
            string shortName,
            string name,
            string allStages,
            int activTable,
            string staffTableFilter,
            string recipientProjectFilter,
            string supervisorProjectFilter,
            string porjectFiltr,
            string filterTaskTable,
            string filterStaffTable
            )
        {
            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }
            var item = new Project
            {
                code = code,
                name = name,
                shortName = shortName,
                priority = priority,
                dateStart = DateTime.Now.AddHours(-5),
                plannedFinishDate = plannedFinishDate,
                supervisor = supervisor,
                link = link,
                archive = "Нет",
                nowStage = allStages == null? "" : allStages.Split(',')[0],
                allStages = allStages,
                history = $"{DateTime.Now.AddHours(-5)} - Проект создан"
            };

            LogisticProject log = new LogisticProject()
            {
                arhive = "Нет",
                projectId = item.id,
                link = link,
                supervisor = supervisor,
                priority = priority,
                allStages = allStages,
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                dateRedaction = DateTime.Now.AddHours(-5),
                comment = "Проект создан"
            };
            _logistickProject.addToDB(log);
            _project.addToDB(item);
            return RedirectToAction("TaskTable", new { activTable = activTable,
                staffTableFilter = staffTableFilter,
                recipientProjectFilter = recipientProjectFilter,
                supervisorProjectFilter = supervisorProjectFilter,
                porjectFiltr = porjectFiltr,
                filterTaskTable = filterTaskTable,
                filterStaffTable = filterStaffTable
            });
        }

        public DateTime redackPriorAndPerenos(string supervisor, DateTime date, TimeSpan plannedTime, string projectCode, string liteTask)
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
                                    redackPriorAndPerenos(task.supervisor, task.date.AddDays(1),task.plannedTime,
                                        task.projectCode, task.liteTask == false ? "Задача" : "Вне очереди"), task.dedline,
                               task.status, task.comment,task.supervisor, task.recipient, task.priority, task.plannedTime, task.start, task.finish, "");

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
                else{
                    return redackPriorAndPerenos(supervisor, date.AddDays(1), plannedTime,
                                        projectCode, liteTask);
                }
            }
            return date;
        }

        public RedirectToActionResult addTaskToDB(
            string code,
            string desc,
            string projectCode,
            string supervisor,
            string recipient,
            string comment,
            TimeSpan plannedTime,
            DateTime date,
            DateTime dedline,
            string Stage,
            string liteTask,
            int activTable,
            string staffTableFilter,
            string recipientProjectFilter,
            string supervisorProjectFilter,
            string porjectFiltr,
            string filterTaskTable,
            string filterStaffTable
            )
        {
            var roleSession = new SessionRoles();
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }

            

            try
            {
                if (projectCode != _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).code)
                {
                    return RedirectToAction("TaskTable", new { activTable = activTable, meesage = "Не коррестный код проекта!",
                        staffTableFilter = staffTableFilter,
                        recipientProjectFilter = recipientProjectFilter,
                        supervisorProjectFilter = supervisorProjectFilter,
                        porjectFiltr = porjectFiltr,
                        filterStaffTable = filterStaffTable
                    });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("TaskTable", new { activTable = activTable, meesage = "Не коррестный код проекта!",
                    staffTableFilter = staffTableFilter,
                    recipientProjectFilter = recipientProjectFilter,
                    supervisorProjectFilter = supervisorProjectFilter,
                    porjectFiltr = porjectFiltr,
                    filterStaffTable = filterStaffTable
                });
            }

            if (plannedTime == TimeSpan.Zero)
            {
                return RedirectToAction("TaskTable", new { activTable = activTable, meesage = "Не указан срок исполнения задачи!",
                    staffTableFilter = staffTableFilter,
                    recipientProjectFilter = recipientProjectFilter,
                    supervisorProjectFilter = supervisorProjectFilter,
                    porjectFiltr = porjectFiltr,
                    filterStaffTable = filterStaffTable
                });
            }

            date = redackPriorAndPerenos(supervisor,date,plannedTime,projectCode,liteTask);

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

            var iid = _appDB.DBTask.FirstOrDefault(p => p.desc == desc).id;
            LogistickTask log = new LogistickTask()
            {
                ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                TaskId = iid,
                descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                resipienId = supervisor != null ? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                dateRedaction = DateTime.Now.AddHours(-5),
                planedTime = plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == "Создана").id,
                comment = "Задача создана. Комментарий: " + comment
            };
            _logistickTask.addToDB(log);

            //var message = new Message(new string[]
            //    { "tailedstory@yandex.ru"}, "Tema yandex", "content yandex");
            //_emailService.SendEmail(message);

            return RedirectToAction("TaskTable", new { activTable = activTable, staffTableFilter = staffTableFilter, 
                recipientProjectFilter= recipientProjectFilter,
                supervisorProjectFilter = supervisorProjectFilter,
                porjectFiltr = porjectFiltr,
                filterTaskTable = filterTaskTable,
                filterStaffTable = filterStaffTable
            });

        }



        
        public ViewResult TaskTable(int Taskid = -1, int Projid = -1, string meesage = "",
            bool TaskRed = false, bool ProjectRed = false, bool ProjectCreate = false,
             string porjectFiltr = "", string supervisorProjectFilter ="",
             string recipientProjectFilter ="", string staffTableFilter ="", int activTable = 0, string filterTaskTable = "",
             string filterStaffTable = "")
        {
            var roleSession = new SessionRoles();
            var sessionCod = "";
            try
            {
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
                sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
            }
            catch (Exception)
            {
                return View(new HomeViewModel());
            }



            

            List<Staff> StaffTable = new List<Staff>();
            if (roleSession.SessionRole == "Директор")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R02"))
                {
                    StaffTable.Add(staffs);
                }
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R04"))
                {
                    StaffTable.Add(staffs);
                }
            }
            else if (roleSession.SessionRole == "ГИП")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R03"))
                {
                    StaffTable.Add(staffs);
                }
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R04"))
                {
                    StaffTable.Add(staffs);
                }
            }
            else if (roleSession.SessionRole == "Помощник ГИПа")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R04"))
                {
                    StaffTable.Add(staffs);
                }
            }
            else if (roleSession.SessionRole == "НО")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R02"))
                {
                    StaffTable.Add(staffs);
                }

                foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleCod == "R05"))
                {
                    StaffTable.Add(staffs);
                }
                foreach (var staff1 in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleCod == "R06"))
                {
                    StaffTable.Add(staff1);
                }
            }
            else if (roleSession.SessionRole == "РГ")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleCod == "R06"))
                {
                    StaffTable.Add(staffs);
                }
            }

            var projects = _project.AllProjects;
            var tasks = _task.AllTasks;
            var staff = StaffTable;

            List<string> staffNames = new List<string>(); 
            staffNames.Add(roleSession.SessionName);
            foreach (var task in staff)
            {
                if (!staffNames.Contains(task.name)) staffNames.Add(task.name);
            }

            List<Tasks> taskStaffTable = tasks.Where(p => staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient)).ToList();

            


            List<string> ollGip = new List<string>();
            foreach(Project proj in projects.OrderBy(p => p.supervisor))
            {
                if(!ollGip.Contains(proj.supervisor)) ollGip.Add(proj.supervisor);
            }

            List<Tasks> tasksTabbleFilter = tasks.Where(p => (staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient))
                                || (p.supervisor == roleSession.SessionName || p.recipient == roleSession.SessionName)).ToList();

            //////  настроить прием фильтров

            List<Tasks> tasksTable = tasks.Where(p => p.creator == roleSession.SessionName).Where(p => staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient)).ToList();

            foreach (var filter in filterStaffTable.Split(','))///
            {
                if (filter == "Все задачи")
                {
                    tasksTable = tasks.Where(p => staffNames.Contains(p.supervisor) || staffNames.Contains(p.recipient)).ToList();
                }
            }


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


            foreach (var filter in staffTableFilter.Split(','))
            {
                if (filter != "" && filter != "Все должности")
                {
                    staff = StaffTable.Where(p => p.post == filter).ToList();
                }
            }
            

            foreach(var filter in porjectFiltr.Split(','))
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


            foreach (var filter in supervisorProjectFilter.Split(','))
            {
                if (filter != "Все ГИПы" && filter != "")
                {
                    projects = projects.Where(p => p.supervisor == filter);
                }
            }


            foreach (var filter in recipientProjectFilter.Split(','))
            {
                if (filter != "Все ответственные" && filter != "")
                {
                    tasks = tasks.Where(p => p.supervisor == filter);
                }
            }

            //////////

            List<string> projCod = new List<string>();
            foreach (Project proj in projects)
            {
                if (!projCod.Contains(proj.code)) projCod.Add(proj.code);
            }
            tasks = tasks.Where(p => projCod.Contains(p.projectCode));
            

            string table1 = "";
            string table2 = "";
            string table3 = "";
            string table4 = "";
            switch (activTable)
            {
                case 0:
                    table1 = "block";
                    table2 = "none";
                    table3 = "none";
                    table4 = "none";
                    break;
                case 1:
                    table1 = "none";
                    table2 = "block";
                    table3 = "none";
                    table4 = "none";
                    break;
                case 2:
                    table1 = "none";
                    table2 = "none";
                    table3 = "block";
                    table4 = "none";
                    break;
                case 3:
                    table1 = "none";
                    table2 = "none";
                    table3 = "none";
                    table4 = "block";
                    break;
            }

            HomeViewModel homeTasks = new HomeViewModel
            {
                session = roleSession,
                roles = _role.AllRoles,
                posts = _post.AllPosts,
                filterStaff = staffTableFilter == "" ? "Все должности" : staffTableFilter,
                filterProj = porjectFiltr == "" ? "Все проекты" : porjectFiltr,
                filterSupProj = supervisorProjectFilter == "" ? "Все ГИПы" : supervisorProjectFilter,
                filterResProj = recipientProjectFilter == "" ? "Все ответственные" : recipientProjectFilter,
                filterTaskTable = filterTaskTable == "" ? "Мои задачи" : filterTaskTable,
                filterStaffTable = filterStaffTable == ""? "Мои задачи" : filterStaffTable, 

                projectTasks = tasks,
                projects = projects,
                projectId = Projid,
                redactedProject = _project.GetProject(Projid),

                tasks = _task.AllTasks,
                taskId = Taskid,
                redactedTask = _task.GetTask(Taskid),
                task4Table = tasksTable,
                task4TableTasks = tasksTabbleFilter,

                staffSess = new Staff(),
                staffs = _staff.AllStaffs,
                staffsTable = staff,
                staffTasks = taskStaffTable,
                staffNames = staffNames,

                ProjectCreate = ProjectCreate,
                TaskRed = TaskRed,
                ProjectRed = ProjectRed,

                nullProject = new Project(),
                nullTask = new Tasks(),
                nullStaff = new Staff(),

                
                activeTable1 = table1,
                activeTable2 = table2,
                activeTable3 = table3,
                activeTable4 = table4,

                activeTableIndex = activTable,

                ollGip = ollGip

            };
            ViewBag.RoleCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;
            ViewBag.Role = roleSession.SessionRole;
            ViewBag.RoleName = roleSession.SessionName;
            ViewBag.ErrorMassage = meesage;
            return View(homeTasks);
        }
    }
    
}
