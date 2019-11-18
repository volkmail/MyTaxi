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
                    var compare_result = (from u in context.Users
                                          where u.UserLogin == login && u.UserPassword == password
                                          select u).ToList();

                    if (compare_result.Count() == 1)
                    {
                        HttpContext.Session.SetString("login", login);
                        HttpContext.Session.SetString("password", password);
                        HttpContext.Session.SetString("isDriver", compare_result[0].DriverSign ? "yes" : "no");

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
