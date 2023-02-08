using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Interfaces
{
    public interface IStaff
    {
        IEnumerable<Staff> AllStaffs { get; }
        IEnumerable<Staff> DivisoinStaff(int divisionId);
        Staff GetStaff(int staffId);

        List<Staff> StaffTable(string SessionRole, string sessionCod);

    }
}
