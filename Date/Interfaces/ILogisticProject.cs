using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Interfaces
{
    public interface ILogisticProject
    {
        IEnumerable<LogisticProject> AllProject { get; }
        IEnumerable<LogisticProject> LogProject(int ProjectId);
        LogisticProject GetLog(int ProjectId);
        void addToDB(LogisticProject logistick);
    }
}
