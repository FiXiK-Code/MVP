using System;
using System.Collections.Generic;
using System.Linq;
using MVP.Date.Models;
using System.Threading.Tasks;
using TaskStatus = MVP.Date.Models.TaskStatus;

namespace MVP.Date.Interfaces
{
    public interface ITaskStatus
    {
        IEnumerable<TaskStatus> AllTaskStatus { get; }
    }
}
