using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVP.Date.Models;
using Tasks = MVP.Date.Models.Tasks;

namespace MVP.Date.Interfaces
{
    public interface ITask
    {
        IEnumerable<Tasks> AllTasks { get; }
        IEnumerable<Tasks> TasksProject(string _projentCode);
        Tasks GetTask(int taskId);
        void addToDB(Tasks task);
        bool redactToDB(int iid,
            DateTime date,
            string status,
            string comment,
            string supervisor,
            string recipient,
            int pririty,
            TimeSpan plannedTime,
            DateTime start,
            DateTime finish);
        bool redactStatus(int id, string stat, string supervisor);
    }
}
