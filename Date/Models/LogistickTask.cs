using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class LogistickTask
    {
        public int id { get; set; }
        public string TaskCode { get; set; }
        public string ProjectCode { get; set; }
        public int TaskId { get; set; }
        public string descTask { get; set; }
        public int supervisorId { get; set; }
        public int resipienId { get; set; }
        public DateTime dateRedaction { get; set; }
        public TimeSpan planedTime { get; set; }
        public TimeSpan actualTime { get; set; }
        public int CommitorId { get; set; }
        public int taskStatusId { get; set; }
        public string comment {get;set;}
    }
}
