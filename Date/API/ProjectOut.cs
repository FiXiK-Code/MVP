using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class ProjectOut
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public int priority { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime plannedFinishDate { get; set; }
        public DateTime actualFinishDate { get; set; }
        public string supervisor { get; set; }
        public int supervisorId { get; set; }
        public string link { get; set; }
        public string history { get; set; }
        public string archive { get; set; }
        public string nowStage { get; set; }
        public string allStages { get; set; }

        public string timeWork { get; set; }
    }
}
