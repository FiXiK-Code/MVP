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
        bool redactToDB(string liteTask,
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
            DateTime finish);
        Task<bool> redactStatusAsync(int id, string stat);

        Task timeWork(int idTask);
        public void bridge(int id);
    }
}
