using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVP.ApiModels;
using MVP.Date.API;
using MVP.Date.Models;
using Tasks = MVP.Date.Models.Tasks;

namespace MVP.Date.Interfaces
{
    public interface ITask
    {
        IEnumerable<Tasks> AllTasks { get; }
        IEnumerable<Tasks> TasksProject(string _projentCode);
        Tasks GetTask(TasksParameters param);
        TasksTableReturnModels GetMoreTasks(List<string> staffNames, SessionRoles roleSession, string filterTable = "");
        void addToDB(Tasks task);
        bool redactToDB(bool liteTask,
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
            string session);
        bool redactStatus(int id, string stat, string session="");

        void timeWork();
    }
}
