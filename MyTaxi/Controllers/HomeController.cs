using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyTaxi.Models;
using Microsoft.AspNetCore.Http;

namespace MyTaxi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["login"] = HttpContext.Session.IsAvailable ? HttpContext.Session.GetString("login") : "Сеанс не существует";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
