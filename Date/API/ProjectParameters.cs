using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class ProjectParameters
    {
        public int id { get; set; }
        public string filterProj { get; set; } 
        public string supervisorFilter { get; set; } = "";

        public string personName { get; set; }
    }
}
