using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class LogistickProjectRep : ILogisticProject
    {
        private readonly AppDB _appDB;

        public LogistickProjectRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<LogisticProject> AllProject => _appDB.DBLogistickProject;

        public IEnumerable<LogisticProject> LogProject(int projectId) => _appDB.DBLogistickProject.Where(p => p.projectId == projectId);

        public LogisticProject GetLog(int projectId) => _appDB.DBLogistickProject.FirstOrDefault(p => p.projectId == projectId);
        public void addToDB(LogisticProject logistick)
        {
            _appDB.DBLogistickProject.Add(logistick);
            _appDB.SaveChanges();
        }
    }
}
