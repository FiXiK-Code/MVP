using Microsoft.EntityFrameworkCore;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks = MVP.Date.Models.Tasks;
using TaskStatus = MVP.Date.Models.TaskStatus;

namespace MVP.Date
{
    public class AppDB : DbContext
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options)
        {

        }

        public DbSet<CompanyStructure> DBCompanyStructure { get; set; }
        public DbSet<Division> DBDivision { get; set; }
        public DbSet<Post> DBPost { get; set; }
        public DbSet<Project> DBProject { get; set; }
        public DbSet<Role> DBRole { get; set; }
        public DbSet<Staff> DBStaff { get; set; }
        public DbSet<Stage> DBStage { get; set; }
        public DbSet<Tasks> DBTask { get; set; }
        public DbSet<TaskStatus> DBTaskStatus { get; set; }
        public DbSet<LogistickTask> DBLogistickTask { get; set; }
        public DbSet<LogisticProject> DBLogistickProject { get; set; }

    }
}
