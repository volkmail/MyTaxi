using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTaxi.Models;
using MyTaxi.Models.ViewModels;

namespace MyTaxi.Controllers
{
    public class TripsController : Controller
    {
        public IActionResult AwaitingTrips()
        {
            AllTrips allTrips = new AllTrips();

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
                            allTrips.isAuthorize = true;
                            allTrips.isDriver = true;
                            allTrips.userName = dataToDictionaryDriver[0].DriverName;
                            allTrips.userSurname = dataToDictionaryDriver[0].DriverSurname;
                            allTrips.userPatronymic = dataToDictionaryDriver[0].DriverPatronymic;
                        }
                    }
                    else
                    {
                        dataToDictionaryClient = context.Clients.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryClient.Count() == 1)
                        {
                            allTrips.isAuthorize = true;
                            allTrips.isDriver = false;
                            allTrips.userName = dataToDictionaryClient[0].ClientName;
                            allTrips.userSurname = dataToDictionaryClient[0].ClientSurname;
                            allTrips.userPatronymic = dataToDictionaryClient[0].ClientPatronymic;
                        }
                    }
                }
            }
            else
            {
                allTrips.isAuthorize = false;
            }
            #endregion

            return View(allTrips);
        }
    }
}
