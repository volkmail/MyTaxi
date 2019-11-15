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
        //protected MyTaxiDbContext pContext = null;
        //public HomeController(MyTaxiDbContext context) => pContext = context;

        public IActionResult Index()
        {
            //Для обращения к базе создаем контекст
            //var context = IoC.MyTaxiDbContext;
            //После чего проверем наличие базы, если ее нет, то автоматически создастся
            //context.Database.EnsureCreated();
            //И дальше из контекста работаем с базой

            using (var context = new MyTaxiDbContext())
            {
                context.Database.EnsureCreated();
            }
            
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
