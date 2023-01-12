using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class CompanyStuctureRep :ICompanyStructure
    {
        private readonly AppDB _appDB;

        public CompanyStuctureRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<CompanyStructure> AllStructures => _appDB.DBCompanyStructure;
    }
}
