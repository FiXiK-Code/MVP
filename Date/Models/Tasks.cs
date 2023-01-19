using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class Tasks
    {
        public int id { get; set; }
        public string code { get; set; }
        public string desc { get; set; }
        public string TaskCodeParent { get; set; }
        public string projectCode { get; set; }
        public string supervisor { get; set; } // name
        public string recipient { get; set; } //name
        public int priority { get; set; }
        public string comment { get; set; }
        public TimeSpan plannedTime { get; set; } //only time
        public TimeSpan actualTime { get; set; }//only time
        public DateTime start { get; set; }
        public DateTime finish { get; set; }
        public DateTime date { get; set; }
        public string Stage { get; set; }
        public bool liteTask { get; set; }
        public string status { get; set; }
        public DateTime startWork { get; set; }
    }
}
