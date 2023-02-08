using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class TaskRep : ITask
    {
        private readonly AppDB _appDB;

        public TaskRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Models.Tasks> AllTasks => _appDB.DBTask;

        public IEnumerable<Models.Tasks> TasksProject(string _projentCode) => _appDB.DBTask.Where(i => i.projectCode == _projentCode);//.Include(p => p.projectCode);

        public Models.Tasks GetTask(int taskId) => _appDB.DBTask.FirstOrDefault(p => p.id == taskId);

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
            DateTime finish)
        {
            if (_appDB.DBTask.Where(p => p.supervisor == supervisor).Where(p => p.recipient == null).Where(p => p.status == "В работе").Count() < 1
                && _appDB.DBTask.Where(p => p.recipient == recipient).Where(p => p.status == "В работе").Count() < 1 
                    || status != "В работе")
            {
                Tasks task = _appDB.DBTask.FirstOrDefault(p => p.id == iid);
                if (task.status == "В работе" && (status == "Выполнена" || status == "На паузе"))
                {
                    task.actualTime += (TimeSpan)(DateTime.Now - task.startWork);
                    task.historyWorc += $"{DateTime.Now.Date.ToString(@"dd\.MM\.yyyy")} в работе: {(DateTime.Now - task.startWork).ToString(@"hh\:mm")}\n";
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

        public async Task<bool> redactStatusAsync(int id, string stat)
        {
            var supervisor = (await _appDB.DBTask.FirstOrDefaultAsync(p => p.id == id)).supervisor;
            var resip = (await _appDB.DBTask.FirstOrDefaultAsync(p => p.id == id)).recipient;
            if (_appDB.DBTask.Where(p => p.supervisor == supervisor).Where(p => p.recipient == null).Where(p => p.status == "В работе").Count() < 1
                && _appDB.DBTask.Where(p => p.recipient == resip).Where(p => p.status == "В работе").Count() < 1
                    || stat != "В работе")
            {
                Tasks task = (await _appDB.DBTask.FirstOrDefaultAsync(p => p.id == id));
                if (task.status == "В работе" && (stat == "Выполнена" || stat == "На паузе"))
                {
                    task.actualTime += (TimeSpan)(DateTime.Now - task.startWork);
                    task.historyWorc += $"{DateTime.Now.Date.ToString(@"dd\.MM\.yyyy")} в работе: {(DateTime.Now - task.startWork).ToString(@"hh\:mm")}\n";
                    Project proj = new Project();
                    try
                    {
                        proj = await _appDB.DBProject.FirstOrDefaultAsync(p => p.code == task.projectCode);
                        proj.timeWork += (TimeSpan)(DateTime.Now - task.startWork);
                    }
                    catch (Exception)
                    {
                        proj = null;
                    }
                    await _appDB.SaveChangesAsync();
                }
                if (stat == "В работе")
                {
                    task.startWork = DateTime.Now;
                }
                if (task.status == "Создана" && stat == "В работе")
                    task.start = DateTime.Now;
                task.status = stat;
                try
                {
                    task.priority = task.liteTask == false ? (await _appDB.DBProject.FirstOrDefaultAsync(p => p.code == task.projectCode)).priority : -1;
                }
                catch (Exception)
                {
                    task.priority = task.liteTask == false ? task.priority : -1;
                }
               
                //if (stat == "Выполнена") task.finish = DateTime.Now;
                _appDB.SaveChanges();
                return true;
            }
            return false;
        }

        // 8 hours
        public async Task timeWork(int idTask)
        {
            var timer = (new TimeSpan(2, 37, 0) - DateTime.Now.TimeOfDay);
            await Task.Delay(timer);
            var test = (await redactStatusAsync(idTask, "На паузе"));
        }

        public void bridge(int id)
        {
            
        }
    }
}
