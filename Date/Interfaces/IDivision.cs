using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Interfaces
{
    public interface IDivision
    {
        IEnumerable<Division> AllDivisions { get;}
    }
}
