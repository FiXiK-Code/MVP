using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class CompanyStructure
    {
        public int id { get; set; }
        public int divisionsId { get; set; }
        public string supervisor { get; set; }

    }
}
