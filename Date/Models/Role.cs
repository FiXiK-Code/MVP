using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class Role
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string supervisor { get; set; }
        public string recipient { get; set; }

    }
}
