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
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Для обращения к базе создаем контекст
            var context = IoC.MyTaxiDbContext;
            //После чего проверем наличие базы, если ее нет, то автоматически создастся
            context.Database.EnsureCreated();
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
