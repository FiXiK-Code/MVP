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
            int iid,
            DateTime date,
            string status,
            string comment,
            string supervisor,
            string recipient,
            int pririty,
            TimeSpan plannedTime,
            DateTime start,
            DateTime finish)
        {
            if (_appDB.DBTask.Where(p => p.recipient == recipient).Where(p => p.status == "В работе").Count() == 0 || status != "В работе")
            {
                Tasks task = _appDB.DBTask.FirstOrDefault(p => p.id == iid);
                task.supervisor = supervisor;
                task.date = date;
                task.recipient = recipient;
                task.comment = comment;
                task.plannedTime = plannedTime;
                task.priority = pririty;
                if (task.status == "Создана" && status == "В работе")
                    task.start = DateTime.Now;
                task.status = status;
                task.finish = finish;
                _appDB.SaveChanges();
                return true;
            }
            else return false;
        }

        public void redactStatus(int id, string stat)
        {
            Tasks task = _appDB.DBTask.FirstOrDefault(p => p.id == id);
            task.status = stat;
            _appDB.SaveChanges();
        }
    }
}
