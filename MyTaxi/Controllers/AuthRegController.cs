using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTaxi.Models;

namespace MyTaxi.Controllers
{
    public class AuthRegController : Controller
    {
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorization(string login, string password)
        {
            using (var context = new MyTaxiDbContext())
            {
                if (context.Users.Any())
                {
                    var compare_result = context.Users.Where(u => u.UserLogin == login && u.UserPassword == password).ToList();

                    if (compare_result.Count() == 1)
                    {
                        HttpContext.Session.SetInt32("userID", compare_result[0].UserID);
                        HttpContext.Session.SetInt32("isDriver", compare_result[0].DriverSign ? 1 : 0);
                        // пока не обязательно HttpContext.Session.SetString("login", login);
                        // пока не обязательно HttpContext.Session.SetString("password", password);
                        return Redirect("/Home/Index");
                    }
                    else
                        return View();
                }
                return View();
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult RegistrationWay()
        {
            return View();
        }

        public IActionResult RegistrationDriver()
        {
            return View();
        }
    }
}
