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
            MyTaxi.Models.ViewModels.HomePage homePageData = new Models.ViewModels.HomePage();

            #region Fill User Data

            if (HttpContext.Session.TryGetValue("userID", out byte[] result))
            {
                using (var context = new MyTaxiDbContext())
                {
                    List<Driver> dataToDictionaryDriver;
                    List<Client> dataToDictionaryClient;

                    if (HttpContext.Session.GetInt32("isDriver") == 1)
                    {
                        dataToDictionaryDriver = context.Drivers.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryDriver.Count() == 1)
                        {
                            homePageData.isAuthorize = true;
                            homePageData.isDriver = true;
                            homePageData.userName = dataToDictionaryDriver[0].DriverName;
                            homePageData.userSurname = dataToDictionaryDriver[0].DriverSurname;
                            homePageData.userPatronymic = dataToDictionaryDriver[0].DriverPatronymic;
                        }
                    }
                    else
                    {
                        dataToDictionaryClient = context.Clients.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryClient.Count() == 1)
                        {
                            homePageData.isAuthorize = true;
                            homePageData.isDriver = false;
                            homePageData.userName = dataToDictionaryClient[0].ClientName;
                            homePageData.userSurname = dataToDictionaryClient[0].ClientSurname;
                            homePageData.userPatronymic = dataToDictionaryClient[0].ClientPatronymic;
                        }
                    }
                }
            }
            else
            {
                homePageData.isAuthorize = false;
            }
            #endregion

            return View(homePageData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
