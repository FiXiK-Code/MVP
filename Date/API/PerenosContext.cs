using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.API
{
    public class PerenosContext
    {
        public DateTime date { get; set; }
        public List<Tasks> tasks { get; set; }
    }
}
