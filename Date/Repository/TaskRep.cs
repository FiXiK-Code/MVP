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
            bool liteTask,
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
            string session = "")
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
                    task.actualTime += (TimeSpan)(DateTime.Now - task.startWork);
                    task.historyWorc += $"{DateTime.Now.AddHours(-5).Date.ToString(@"dd\.MM\.yyyy")} в работе: {(DateTime.Now - task.startWork).ToString(@"hh\:mm")}\n";
                    Project proj = new Project();
                    try
                    {
                        proj = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode);
                        proj.timeWork += (TimeSpan)(DateTime.Now - task.startWork);
                    }
                    catch (Exception)
                    {
                        proj = null;
                    }
                    _appDB.SaveChanges();
                }
                if (status == "В работе")
                {
                    task.startWork = DateTime.Now;
                }
                task.supervisor = supervisor;
                task.date = date;
                task.dedline = dedline;
                task.recipient = recipient;
                task.comment += comment != null? comment+ "\n": null;
                task.plannedTime = plannedTime;
                if (task.status == "Создана" && status == "В работе")
                    task.start = DateTime.Now;
                task.status = status;
                if(status == "Выполнена")
                {
                    task.finish = DateTime.Now;
                }else task.finish = finish;

                task.liteTask = liteTask;
                try
                {
                    task.priority = liteTask == false ? _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode).priority : -1;
                }
                catch (Exception)
                {
                    task.priority = liteTask == false ? pririty : -1;
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
                    task.actualTime += (TimeSpan)(DateTime.Now - task.startWork);
                    task.historyWorc += $"{DateTime.Now.AddHours(-5).Date.ToString(@"dd\.MM\.yyyy")} в работе: {(DateTime.Now - task.startWork).ToString(@"hh\:mm")}\n";
                    Project proj = new Project();
                    try
                    {
                        proj = _appDB.DBProject.FirstOrDefault(p => p.code == task.projectCode);
                        proj.timeWork += (TimeSpan)(DateTime.Now - task.startWork);
                    }
                    catch (Exception)
                    {
                        proj = null;
                    }
                    _appDB.SaveChanges();
                }
                if (stat == "В работе")
                {
                    task.startWork = DateTime.Now;
                }
                if (task.status == "Создана" && stat == "В работе")
                    task.start = DateTime.Now;
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

                if (stat == "Выполнена") task.finish = DateTime.Now;
                _appDB.SaveChanges();
                return true;
            }
            return false;
        }    
                
        

        // 8 hours
        public void timeWork()
        {
            foreach(var task in _appDB.DBTask.Where(p => p.status == "В работе"))
            {
                redactStatus(task.id,"На паузе");
            }
        }

        public TasksTableReturnModels GetMoreTasks(List<string> staffNames, SessionRoles roleSession, string filterTable = "Мои задачи", List<string> projCode = null)
        {
            List<Tasks> tasksTabbleFilter = new List<Tasks>();
            if (projCode != null) tasksTabbleFilter.AddRange(AllTasks.Where(p => projCode.Contains(p.projectCode)).ToList());
            if (staffNames != null) tasksTabbleFilter = tasksTabbleFilter.Where(p => ((staffNames.Contains(p.supervisor) && p.recipient == null) || staffNames.Contains(p.recipient))).ToList();

            // редактирование возвращаемых задач в зависимости от фильтра (в перспективе передача нескольких фильтров через запятую)
            if (filterTable != "")
                foreach (var filter in filterTable.Split(','))///
                {
                    switch (filter)
                    {
                        case "Мои задачи":
                            tasksTabbleFilter.AddRange(AllTasks.Where(p => p.recipient == roleSession.SessionName
                            || p.supervisor == roleSession.SessionName).ToList());
                            break;
                    }
                }

            var today = tasksTabbleFilter.Where(p => p.status != "Выполнена").Where(p => p.date.Date <= DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList();

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
                    if(!todayOut.Contains(outt))todayOut.Add(outt);
                }
                catch (Exception)
                {
                    continue;
                }

            }



            // выполненные задачи
            var completed = tasksTabbleFilter.Where(p => p.status == "Выполнена").OrderBy(p => p.finish).ToList();

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
                    if(!completedOut.Contains(outt)) completedOut.Add(outt);
                }
                catch (Exception)
                {
                    continue;
                }


            }
            // будущие задачи 
            var future = tasksTabbleFilter.Where(p => p.date.Date > DateTime.Now.Date).OrderBy(p => p.date.Date).OrderBy(p => p.priority).ToList();

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
                    if (!futureOut.Contains(outt)) futureOut.Add(outt);
                }
                catch (Exception)
                {
                    continue;
                }
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
