using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP.Date;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using MVP.ViewModels;
using Newtonsoft.Json;
using System;
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

        public RedirectToActionResult RedactSatusTask(int id, string stat)
        {
            _task.redactStatus(id, stat);
            return RedirectToAction("TaskTable");
        }

        public RedirectToActionResult RedactProjectToDB(
            int iid,
            string arhive,
            string link,
            string supervisor,
            int priority,
            string allStages,
            string comment
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
            return RedirectToAction("TaskTable", new { Projid = iid});

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
            DateTime finish
)
        {

            if (!_task.redactToDB(iid, date, status, comment, supervisor, recipient, pririty, plannedTime, start, finish))
            {
                var msg = "Только одна задача может быть в работе! Проверьте статусы своих задачь!";
                return RedirectToAction("TaskTable", new { Taskid = iid, meesage = msg, TaskRed = true });
            }
            else
            {
                _project.NextStage(_appDB.DBProject.FirstOrDefault(p => p.code == _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode).id);

                var roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));

                LogistickTask item = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                    TaskId = iid,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                    resipienId = _appDB.DBStaff.FirstOrDefault(p => p.name == recipient).id,
                    dateRedaction = DateTime.Now,
                    planedTime = plannedTime,
                    actualTime = new TimeSpan(),
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == status).id,
                    comment = comment

                };
                _logistickTask.addToDB(item);
                if (status == "Создана") await TimerPauseTask(iid);
                return RedirectToAction("TaskTable", new { Taskid = iid });
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
            string allStages
           
            )
        {

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
                nowStage = allStages.Split(',')[0],
                allStages =allStages,
                history = $"{DateTime.Now} - Проект создан"
            };
            _project.addToDB(item);
            return RedirectToAction("TaskTable");
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
            string liteTask
            )
        {
            var roleSession = JsonConvert.DeserializeObject<SessionRoles>(HttpContext.Session.GetString("Session"));
            var staff = _staff.AllStaffs.FirstOrDefault(p => p.name == recipient).post;
            var cod = _post.AllPosts.FirstOrDefault(p => p.name == staff).roleCod;
            var staffRoleId = _role.AllRoles.FirstOrDefault(p => p.code == cod).id;
            if (_role.AllRoles.FirstOrDefault(p => p.name == roleSession.SessionRole).id <= staffRoleId)
            {
                return RedirectToAction("TaskTable", new { meesage = "Можно выбрать подчиненного только в соответсвии с вашей ролью!"});
            }
            else
            {
                try
                {
                    if (projectCode != _appDB.DBProject.FirstOrDefault(p => p.code == projectCode).code)
                    {
                        return RedirectToAction("TaskTable", new { meesage = "Не коррестный код проекта!" });
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("TaskTable", new { meesage = "Не коррестный код проекта!" });
                }
                var item = new Tasks
                {
                    code = code,
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

                var iid = _appDB.DBTask.FirstOrDefault(p => p.code == code).id;
                LogistickTask log = new LogistickTask()
                {
                    ProjectCode = _appDB.DBTask.FirstOrDefault(p => p.id == iid).projectCode,
                    TaskId = iid,
                    descTask = _appDB.DBTask.FirstOrDefault(p => p.id == iid).desc,
                    supervisorId = _appDB.DBStaff.FirstOrDefault(p => p.name == supervisor).id,
                    resipienId = _appDB.DBStaff.FirstOrDefault(p => p.name == recipient).id,
                    dateRedaction = DateTime.Now,
                    planedTime = plannedTime,
                    actualTime = new TimeSpan(),
                    CommitorId = _appDB.DBStaff.FirstOrDefault(p => p.name == roleSession.SessionName).id,
                    taskStatusId = _appDB.DBTaskStatus.FirstOrDefault(p => p.name == "Создана").id,
                    comment = comment
                };
                _logistickTask.addToDB(log);


                return RedirectToAction("TaskTable");
            }
        }



        
        public ViewResult TaskTable(int Taskid = -1, int Projid = -1, string meesage = "",
            bool TaskRed = false, bool ProjectRed = false, bool ProjectCreate = false,
             string porjectFiltr = "", string supervisorProjectFilter ="",
             string recipientProjectFilter ="", string staffTableFilter ="")
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
            

            
            var projects = _project.AllProjects;
            var tasks = _task.AllTasks;
            var staff = _staff.AllStaffs;
            if (staffTableFilter!="" && staffTableFilter != "Все должности")
            {
                staff = staff.Where(p => p.post == staffTableFilter);
            }
            if (porjectFiltr == "Проекты в архиве")
            {
                projects =  projects.Where(p => p.archive == "Да");
            }
            if (supervisorProjectFilter != "Все ГИПы" && supervisorProjectFilter !="")
            {
                projects = projects.Where(p => p.supervisor == supervisorProjectFilter);
            }
            if(recipientProjectFilter != "Все исполнители" && recipientProjectFilter != "")
            {
                tasks = tasks.Where(p => p.recipient == recipientProjectFilter);
            }

            HomeViewModel homeTasks = new HomeViewModel
            {
                session = roleSession,
                roles = _role.AllRoles,
                posts = _post.AllPosts,
                filterStaff = staffTableFilter == "" ? "Все должности" : staffTableFilter,
                filterProj = porjectFiltr == "" ? "Все проекты" : porjectFiltr,
                filterSupProj = supervisorProjectFilter == "" ? "Все ГИПы" : supervisorProjectFilter,
                filterResProj = recipientProjectFilter == "" ? "Все исполнители" : recipientProjectFilter,

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
                nullStaff = new Staff()
            };

            ViewBag.Role = roleSession.SessionRole;
            ViewBag.RoleName = roleSession.SessionName;
            ViewBag.ErrorMassage = meesage;
            return View(homeTasks);
        }
    }
    
}
