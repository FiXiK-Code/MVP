using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class Stage
    {
        public int id { get; set; }
        public int stageId { get; set; }
        public string name { get; set; }
        public int projectId { get; set; }

    }
}
