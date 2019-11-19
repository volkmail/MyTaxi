using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyTaxi.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace MyTaxi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetValue("userID", out byte[] result))
            {
                using (var context = new MyTaxiDbContext())
                {
                    List<Driver> dataToDictionaryDriver;
                    List<Client> dataToDictionaryClient;
                    Dictionary<string, string> userData = null;

                    if (HttpContext.Session.GetInt32("isDriver") == 1)
                    {
                        userData = new Dictionary<string, string>();
                        dataToDictionaryDriver = context.Drivers.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryDriver.Count() == 1)
                        {
                            userData.Add("isDriver", "yes");
                            userData.Add("userName", dataToDictionaryDriver[0].DriverName);
                            userData.Add("userSurname", dataToDictionaryDriver[0].DriverSurname);
                            userData.Add("userPatronymic", dataToDictionaryDriver[0].DriverPatronymic);
                        }
                    }
                    else
                    {
                        userData = new Dictionary<string, string>();
                        dataToDictionaryClient = context.Clients.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryClient.Count() == 1)
                        {
                            userData.Add("isDriver", "no");
                            userData.Add("userName", dataToDictionaryClient[0].ClientName);
                            userData.Add("userSurname", dataToDictionaryClient[0].ClientSurname);
                            userData.Add("userPatronymic", dataToDictionaryClient[0].ClientPatronymic);
                        }
                    }
                    return View(userData);
                }
            }
            else
                return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
