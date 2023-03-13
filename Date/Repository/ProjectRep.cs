using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class ProjectRep : IProject
    {
        private readonly AppDB _appDB;

        public ProjectRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Project> AllProjects => _appDB.DBProject;



        public Project GetProject(int projectId)
        {
            try 
            {
                return _appDB.DBProject.FirstOrDefault(p => p.id == projectId);
            }
            catch
            {
                return null;
            }
        }

        public void NextStage(int projectId)
        {
            if (projectId != -1)
            {
                if (_appDB.DBTask.Where(p => p.liteTask == false)
                    .Where(i => i.Stage == _appDB.DBProject.FirstOrDefault(p => p.id == projectId).nowStage)
                    .Where(i => i.status != "Выполнена").Count() == 0 && projectId != -1)
                {
                    Project proj = _appDB.DBProject.FirstOrDefault(p => p.id == projectId);
                    if (_appDB.DBStage.Where(p => p.stageId == projectId)
                        .FirstOrDefault(
                        p => p.stageId ==
                            (_appDB.DBStage.Where(p => p.projectId == projectId)
                            .FirstOrDefault(p => p.name == proj.nowStage)
                            .stageId + 1)) == null)
                    {
                        proj.archive = "Да";
                        proj.nowStage = "Проект в архиве";
                        proj.actualFinishDate = DateTime.Now.AddHours(-5);
                    }
                    else
                    {
                        proj.nowStage = _appDB.DBStage.Where(p => p.stageId == projectId)
                            .FirstOrDefault(
                            p => p.stageId ==
                                (_appDB.DBStage.Where(p => p.projectId == projectId)
                                .FirstOrDefault(p => p.name == proj.nowStage)
                                .stageId + 1)
                            ).name;
                        proj.history = proj.history + $"\n{DateTime.Now.AddHours(-5)} - Проект перешел в стадию {proj.nowStage}";
                        _appDB.SaveChanges();
                    }
                }
            }
        }
        public void addToDB(Project project)
        {
            _appDB.DBProject.Add(project);
            _appDB.SaveChanges();
            var i = 0;
            if(project.allStages != null)
            {
                foreach (var stage in project.allStages.Split(','))
                {
                    var elem = new Stage()
                    {
                        stageId = i,
                        name = stage,
                        projectId = project.id
                    };
                    _appDB.DBStage.Add(elem);
                    _appDB.SaveChanges();
                    i++;
                }
            }
            
            
        }

        public void redactToDB(int id,
            string code,
            string shortName,
            string name,
            string arhive,
            string link,
            string supervisor,
            int priority,
            string allStages)
        {
            Project project = _appDB.DBProject.FirstOrDefault(p => p.id == id);
            if (arhive == "Да") project.actualFinishDate = DateTime.Now;
            project.shortName = shortName;
            project.name = name;
            project.code = code;
            project.archive = arhive;
            project.link = link;
            project.supervisor = supervisor;
            project.priority = priority;
            project.allStages = allStages;
            project.history += $"\n{DateTime.Now.AddHours(-5)} - В проект внесли изменения";
            _appDB.SaveChanges();
        }
    }
}
