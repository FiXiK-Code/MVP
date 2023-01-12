using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class LogisticProject
    {
        public int id { get; set; }
        public int projectId { get; set; }
        public string arhive { get; set; }
        public string link { get; set; }
        public string supervisor { get; set; }
        public int priority { get; set; }
        public string allStages { get; set; }
        public int CommitorId { get; set; }
        public DateTime dateRedaction { get; set; }
        public string comment { get; set; }
    }
}
