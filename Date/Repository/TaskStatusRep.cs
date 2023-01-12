using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class TaskStatusRep : ITaskStatus
    {
        private readonly AppDB _appDB;

        public TaskStatusRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Models.TaskStatus> AllTaskStatus => _appDB.DBTaskStatus;
    }
}
