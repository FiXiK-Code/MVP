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

        public HomeController(IPost post, IRole role,ITask task, IProject project, AppDB appDB, ILogistickTask logistick, IStaff staff, ILogisticProject logistickProject)
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

        public RedirectToActionResult RedactSatusTask(int id, string stat,int activTable)
        {
            _task.redactStatus(id, stat);
            return RedirectToAction("TaskTable", new { activTable = activTable });
        }

        public RedirectToActionResult RedactProjectToDB(
            int iid,
            string arhive,
            string link,
            string supervisor,
            int priority,
            string allStages,
            string comment,
            int activTable
            )
        {
            _project.redactToDB(iid, arhive,link,supervisor,priority,allStages);

            var roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));

            LogisticProject item = new LogisticProject()
            {
                arhive = arhive,
                projectId = iid,
                link = link,
                supervisor = supervisor,
                priority = priority,
                allStages = allStages,
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                dateRedaction = DateTime.Now,
                comment = comment
            };

            _logistickProject.addToDB(item);
            return RedirectToAction("TaskTable", new { activTable = activTable ,Projid = iid});

        }
        public async Task<RedirectToActionResult> RedactTaskToDB(
            int iid,
            DateTime date,
            string status,
            string comment,
            string supervisor,
            string recipient,
            int pririty,
            TimeSpan plannedTime,
            DateTime start,
            DateTime finish,
            int activTable
)
        {

            if (!_task.redactToDB(iid, date, status, comment, supervisor, recipient, pririty, plannedTime, start, finish))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return RedirectToAction("TaskTable", new { activTable = activTable, Taskid = iid, meesage = msg, TaskRed = true });
            }
            else
            { 
                var projCod = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode;
                var projId = _appDB.DBProject.FirstOrDefault(p => p.code == projCod) != null ? _appDB.DBProject.FirstOrDefault(p => p.code == projCod).id : -1;
                _project.NextStage(projId);

                var roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));

                LogistickTask item = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                    TaskId = iid,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                    resipienId = supervisor != null? _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id : -1,
                    dateRedaction = DateTime.Now,
                    planedTime = plannedTime,
                    actualTime = new TimeSpan(),
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == status).id,
                    comment = comment
                };
                _logistickTask.addToDB(item);
                if (status == "Создана") await TimerPauseTask(iid);
                return RedirectToAction("TaskTable", new { activTable = activTable, Taskid = iid });
            }

        }

        public async Task TimerPauseTask(int idTask)
        {
            await Task.Delay(43200000);
            Tasks el =  _appDB.DBTask.FirstOrDefault(p => p.id == idTask);
            el.status = "В работе";
            await _appDB.SaveChangesAsync();
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
            int activTable
            )
        {
            var roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));

            var item = new Project
            {
                code = code,
                name = name,
                shortName = shortName,
                priority = priority,
                dateStart = DateTime.Now,
                plannedFinishDate = plannedFinishDate,
                supervisor = supervisor,
                link = link,
                archive = "Нет",
                nowStage = allStages == null? "" : allStages.Split(',')[0],
                allStages = allStages,
                history = $"{DateTime.Now} - Проект создан"
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
                dateRedaction = DateTime.Now,
                comment = "Проект создан"
            };
            _logistickProject.addToDB(log);
            _project.addToDB(item);
            return RedirectToAction("TaskTable", new { activTable = activTable});
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
            string Stage,
            string liteTask,
            int activTable
            )
        {
            var roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));

            try
            {
                if (projectCode != _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).code)
                {
                    return RedirectToAction("TaskTable", new { activTable = activTable, meesage = "Не коррестный код проекта!" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("TaskTable", new { activTable = activTable, meesage = "Не коррестный код проекта!" });
            }
            var item = new Tasks
            {
                actualTime = TimeSpan.Zero,
                desc = desc,
                projectCode = projectCode,
                supervisor = supervisor,
                recipient = recipient,
                priority = _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).priority,
                comment = comment,
                plannedTime = plannedTime,
                date = date,
                Stage = Stage,
                status = "Создана",
                liteTask = liteTask == "Подзадача" ? true : false
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
                dateRedaction = DateTime.Now,
                planedTime = plannedTime,
                actualTime = new TimeSpan(),
                CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == "Создана").id,
                comment = comment
            };
            _logistickTask.addToDB(log);


            return RedirectToAction("TaskTable", new { activTable = activTable });

        }



        
        public ViewResult TaskTable(int Taskid = -1, int Projid = -1, string meesage = "",
            bool TaskRed = false, bool ProjectRed = false, bool ProjectCreate = false,
             string porjectFiltr = "", string supervisorProjectFilter ="",
             string recipientProjectFilter ="", string staffTableFilter ="", int activTable = 0 )
        {
            var roleSession = new SessionRoles();
            try
            { 
                roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            }
            catch (Exception)
            {
                roleSession = new SessionRoles();
            }



            var sessionCod = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).code;

            List<Staff> StaffTable = new List<Staff>();
            List<Staff> RG = new List<Staff>();
            if (roleSession.SessionRole == "Директор")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleId == 5))
                {
                    StaffTable.Add(staffs);
                }
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleId == 3))
                {
                    StaffTable.Add(staffs);
                }
            }
            else if (roleSession.SessionRole == "ГИП")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleId == 6))
                {
                    StaffTable.Add(staffs);
                }
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleId == 3))
                {
                    StaffTable.Add(staffs);
                }
            }
            else if (roleSession.SessionRole == "Помощник ГИПа")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleId == 3))
                {
                    StaffTable.Add(staffs);
                }
            }
            else if (roleSession.SessionRole == "НО")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.roleId == 5))
                {
                    StaffTable.Add(staffs);
                }

                foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleId == 1))
                {
                    StaffTable.Add(staffs);
                    RG.Add(staffs);
                }
                foreach (var staffs in RG)
                {
                    foreach (var staff1 in _appDB.DBStaff.Where(p => p.supervisorCod == staffs.code && p.roleId == 2))
                    {
                        StaffTable.Add(staff1);
                    }
                }
            }
            else if (roleSession.SessionRole == "РГ")
            {
                foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleId == 2))
                {
                    StaffTable.Add(staffs);
                }
            }

            var projects = _project.AllProjects;
            var tasks = _task.AllTasks;
            var staff = StaffTable;

            

            List<string> ollGip = new List<string>();
            foreach(Project proj in projects.OrderBy(p => p.supervisor))
            {
                if(!ollGip.Contains(proj.supervisor)) ollGip.Add(proj.supervisor);
            }

            if (staffTableFilter!="" && staffTableFilter != "Все должности")
            {
                staff = StaffTable.Where(p => p.post == staffTableFilter).ToList();
            }
            if (porjectFiltr == "Проекты в архиве")
            {
                projects =  projects.Where(p => p.archive == "Да");
            }
            if (supervisorProjectFilter != "Все ГИПы" && supervisorProjectFilter !="")
            {
                projects = projects.Where(p => p.supervisor == supervisorProjectFilter);
            }
            if(recipientProjectFilter != "Все ответственные" && recipientProjectFilter != "")
            {
                tasks = tasks.Where(p => p.supervisor == recipientProjectFilter);
            }
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

                projectTasks = tasks,
                projects = projects,
                projectId = Projid,
                redactedProject = _project.GetProject(Projid),

                tasks = _task.AllTasks,
                taskId = Taskid,
                redactedTask = _task.GetTask(Taskid),

                staffSess = new Staff(),
                staffs = _staff.AllStaffs,
                staffsTable = staff,

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
