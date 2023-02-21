﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class ProjectParameters
    {
        public int id { get; set; } = -1;
        public string filterProj { get; set; } = "";
        public string supervisorFilter { get; set; } = "";


        public string link { get; set; }
        public  string arhive { get; set; }
        public string code {get; set; }
        public int supervisor {get; set; }
        public int priority {get; set; }
        public  string  comment { get; set; }
        public string plannedFinishDate {get; set; }
        public string shortName {get; set; }
        public string name {get; set; }
        public string allStages {get; set; }
    }
}
