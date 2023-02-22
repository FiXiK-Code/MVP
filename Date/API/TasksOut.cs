using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class TasksOut
    {
        public int id { get; set; }
        public string code { get; set; }
        public string desc { get; set; }
        public string TaskCodeParent { get; set; }
        public string projectCode { get; set; }
        public int projectId { get; set; }
        public string supervisor { get; set; } // name
        public int supervisorId { get; set; }
        public string recipient { get; set; } //name
        public int recipientId { get; set; }
        public int priority { get; set; }
        public string comment { get; set; }
        public string plannedTime { get; set; } //time
        public string actualTime { get; set; } //time
        public string start { get; set; }//oll
        public string finish { get; set; }//oll
        public string date { get; set; }//date
        public string Stage { get; set; }
        public bool liteTask { get; set; }
        public string status { get; set; }
        public DateTime startWork { get; set; }//oll
        public string creator { get; set; }
        public string historyWorc { get; set; }

        public string dedline { get; set; }//oll
    }
}
