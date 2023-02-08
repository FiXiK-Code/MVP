using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.ViewModels
{
    public class HomeViewModel
    {
        public string filterStaff { get; set; }
        public string filterProj { get; set; }
        public string filterSupProj { get; set; }
        public string filterResProj { get; set; }
        public IEnumerable<Tasks> projectTasks { get; set; }
        public IEnumerable<Tasks> tasks { get; set; }
        public int taskId { get; set; }
        public Tasks redactedTask { get; set; }
        public Tasks nullTask { get; set; }

        public List<Tasks> task4Table { get; set; }

        public List<Tasks> task4TableTasks { get; set; }

        public string filterTaskTable { get; set; }
         public string filterStaffTable { get; set; }



        public IEnumerable<Project> projects { get; set; }
        public int projectId { get; set; }
        public Project redactedProject { get; set; }
        public Project nullProject { get; set; }

        public Staff nullStaff { get; set; }

        public Staff staffSess { get; set; }
        public IEnumerable<Staff> staffs { get; set; }
        public IEnumerable<Staff> staffsTable { get; set; }
        public IEnumerable<Role> roles { get; set; }
        public IEnumerable<Post> posts { get; set; }
        public IEnumerable<Tasks> staffTasks { get; set; }

        public List<string> staffNames{get;set;}

        public bool TaskRed { get; set; }
        public bool ProjectRed { get; set; }
        public bool ProjectCreate { get; set; }
        

        public SessionRoles session { get; set; }


        public string activeTable1 { get; set; }
        public string activeTable2 { get; set; }
        public string activeTable3 { get; set; }

        public string activeTable4 { get; set; }

        public int activeTableIndex { get; set; }

        public List<string> ollGip { get; set; }
    }
}
