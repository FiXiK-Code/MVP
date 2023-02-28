using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class TasksParameters // насторить необходимые фильтры
    {
        public int id { get; set; } = -1;
        
        public string status { get; set; }
        public string desc { get; set; }
        public int projectCode { get; set; }
        public int supervisor { get; set; }
        public int recipient { get; set; }
        public string comment { get; set; }
        
        public int pririty { get; set; }
        public string plannedTime { get; set; }
        public string date { get; set; }
        public string dedline { get; set; }
        public string start { get; set; }
        public string finish { get; set; }
        public string Stage { get; set; }
        public bool liteTask { get; set; }


    }
}
