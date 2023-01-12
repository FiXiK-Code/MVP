using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Interfaces
{
    public interface IProject
    {
        IEnumerable<Project> AllProjects { get;}

        Project GetProject(int projectId);

        public void NextStage(int projectId);
        void addToDB(Project project);
        void redactToDB(int id,
            string arhive,
            string link,
            string supervisor,
            int priority,
            string allStages);//посмотерть из шаблона ProjectRedact какие параметры передавать

    }
}
