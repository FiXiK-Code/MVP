using Microsoft.EntityFrameworkCore;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class RoleRep : IRole
    {

        private readonly AppDB _appDB;

        public RoleRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public IEnumerable<Role> AllRoles => _appDB.DBRole;
    }
}
