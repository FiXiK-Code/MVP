using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class StageRep : IStage
    {
        private readonly AppDB _appDB;

        public StageRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Stage> AllStages => throw new NotImplementedException();

        public Stage GetStage(int projectId) => _appDB.DBStage.FirstOrDefault(p => p.projectId == projectId);

        public IEnumerable<Stage> GetStagesProject(int projectId) => _appDB.DBStage.Where(i => i.projectId == projectId);//.Include(p => p.projectId);
    }
}
