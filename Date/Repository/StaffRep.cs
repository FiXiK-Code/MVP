using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class StaffRep : IStaff
    {
        private readonly AppDB _appDB;

        public StaffRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Staff> AllStaffs => _appDB.DBStaff;

        public IEnumerable<Staff> DivisoinStaff(int divisionId) => _appDB.DBStaff.Where(p => p.divisionId == divisionId);// incl

        public Staff GetStaff(int staffId) => _appDB.DBStaff.FirstOrDefault(p => p.id == staffId);
    }
}
