using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP.Date;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using MVP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDB _appDB;
        private readonly IStaff _staff;
        public LoginController(IStaff staff,AppDB appDB)
        {
            _staff = staff;
            _appDB = appDB;
        }

        public ViewResult Index(string message = "")
        {
            
            ViewBag.Error = message;
            return View(new Staff());
        }

        public RedirectToActionResult InitSession(string login,string password)
        {
            if(_appDB.DBStaff.FirstOrDefault(p => p.login == login) == null || _appDB.DBStaff.FirstOrDefault(p => p.login == login).passvord != password) return RedirectToAction("Index", new { message = "Данные не корректы! Повторите попытку!"});
            else{
                if (password == null) return RedirectToAction("Index", new { message = "Данные не корректы! Повторите попытку!" });
                else
                {
                    var post = _appDB.DBStaff.FirstOrDefault(p => p.login== login).post;
                    var roleId = _appDB.DBPost.FirstOrDefault(p => p.name== post).roleCod;
                    var sessionInit = new SessionRoles()
                    {
                        SessionName = _appDB.DBStaff.FirstOrDefault(p => p.login == login).name,
                        SessionRole = _appDB.DBRole.FirstOrDefault(p => p.code == roleId).name
                    };
                    HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(sessionInit));
                    return RedirectToAction("TaskTable","Home");
                }
            }
            
        }
    }
}
