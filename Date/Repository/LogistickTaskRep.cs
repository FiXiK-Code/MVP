using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{

    public class LogistickTaskRep : ILogistickTask
    {
        private readonly AppDB _appDB;

        public LogistickTaskRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<LogistickTask> AllTasks => _appDB.DBLogistickTask;
        public IEnumerable<LogistickTask> LogTask(int TaskId) => _appDB.DBLogistickTask.Where(p => p.TaskId == TaskId);

        public LogistickTask GetLog(int taskId) => _appDB.DBLogistickTask.FirstOrDefault(p => p.TaskId == taskId);
        public void addToDB(LogistickTask logistick)
        {
            _appDB.DBLogistickTask.Add(logistick);
            _appDB.SaveChanges();
        }

        

       
    }
}
