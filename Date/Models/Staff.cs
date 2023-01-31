using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Models
{
    public class Staff
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int divisionId { get; set; }
        public string post { get; set; }
        public string roleCod { get; set; }
        public string supervisorCod { get; set; }
        //private??
        public string login { get; set; }
        public string passvord { get; set; }

        public string mail { get; set; }
    }
}
