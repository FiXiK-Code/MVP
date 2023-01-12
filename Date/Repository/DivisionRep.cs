using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class DivisionRep :IDivision
    {
        private readonly AppDB _appDB;

        public DivisionRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Division> AllDivisions => _appDB.DBDivision;
    }
}
