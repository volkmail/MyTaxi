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
    public class HistoryController : Controller
    {
        public IActionResult History()
        {
            Models.ViewModels.History history = new Models.ViewModels.History();

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
                            history.isAuthorize = true;
                            history.isDriver = true;
                            history.userName = dataToDictionaryDriver[0].DriverName;
                            history.userSurname = dataToDictionaryDriver[0].DriverSurname;
                            history.userPatronymic = dataToDictionaryDriver[0].DriverPatronymic;
                        }
                    }
                    else
                    {
                        dataToDictionaryClient = context.Clients.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryClient.Count() == 1)
                        {
                            history.isAuthorize = true;
                            history.isDriver = false;
                            history.userName = dataToDictionaryClient[0].ClientName;
                            history.userSurname = dataToDictionaryClient[0].ClientSurname;
                            history.userPatronymic = dataToDictionaryClient[0].ClientPatronymic;
                        }
                    }
                }
            }
            else
            {
                history.isAuthorize = false;
            }
            #endregion

            return View(history);
        }
    }
}
