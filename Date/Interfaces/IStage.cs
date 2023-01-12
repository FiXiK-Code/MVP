using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Interfaces
{
    public interface IStage
    {
        IEnumerable<Stage> AllStages { get; }
        IEnumerable<Stage> GetStagesProject(int projectId);
        Stage GetStage(int projectId);
    }
}
