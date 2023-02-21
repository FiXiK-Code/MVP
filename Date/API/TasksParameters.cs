using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class TasksParameters // насторить необходимые фильтры
    {
        public int id { get; set; } = -1;
        public string filterTable { get; set; } = "";

        public string status { get; set; }
        public string desc { get; set; }
        public string projectCode { get; set; }
        public string supervisor { get; set; }
        public string recipient { get; set; }
        public string comment { get; set; }
        
        public int pririty { get; set; }
        public TimeSpan plannedTime { get; set; }
        public DateTime date { get; set; }
        public DateTime dedline { get; set; }
        public DateTime start { get; set; }
        public DateTime finish { get; set; }
        public string Stage { get; set; }
        public string liteTask { get; set; }


    }
}
