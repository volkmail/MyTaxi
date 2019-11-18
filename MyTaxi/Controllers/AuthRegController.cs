using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
