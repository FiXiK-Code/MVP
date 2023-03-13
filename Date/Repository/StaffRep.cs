using Microsoft.EntityFrameworkCore;
using MVP.Date.API;
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

        public List<StaffOut> StaffTable(string SessionRole, string sessionCod)
        {
            List<StaffOut> StaffTable = new List<StaffOut>();

            var staff = _appDB.DBStaff.FirstOrDefault(p => p.code == sessionCod);
            var obj = new StaffOut
            {
                id = staff.id,
                code = staff.code,
                name = staff.name,
                divisionId = staff.divisionId,
                post = staff.post,
                roleCod = staff.roleCod,
                supervisorCod = staff.supervisorCod,
                login = staff.login,
                mail = staff.mail
            };
            StaffTable.Add(obj);
            switch (SessionRole)
            {
                case "Директор":
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R02"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R04"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }

                    break;
                case "ГИП":
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R03"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R04"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }


                    break;
                case "Помощник ГИПа":
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R04"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }
                    break;
                case "НО":
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.roleCod == "R02"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleCod == "R05"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleCod == "R06"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }

                    break;
                case "РГ":
                    foreach (var staffs in _appDB.DBStaff.Where(p => p.supervisorCod == sessionCod && p.roleCod == "R06"))
                    {
                        var outStaff = new StaffOut
                        {
                            id = staffs.id,
                            code = staffs.code,
                            name = staffs.name,
                            divisionId = staffs.divisionId,
                            post = staffs.post,
                            roleCod = staffs.roleCod,
                            supervisorCod = staffs.supervisorCod,
                            login = staffs.login,
                            mail = staffs.mail
                        };
                        StaffTable.Add(outStaff);
                    }
                    
                    break;
            }
            return StaffTable;
        }
    }
}
