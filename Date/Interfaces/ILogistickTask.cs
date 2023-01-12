using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Interfaces
{
    public  interface ILogistickTask
    {
        IEnumerable<LogistickTask> AllTasks { get; }
        IEnumerable<LogistickTask> LogTask(int TaskId);
        LogistickTask GetLog(int taskId);
        void addToDB(LogistickTask logistick);
    }
}
