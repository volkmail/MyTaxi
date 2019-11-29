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
using Newtonsoft.Json;

namespace MyTaxi.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Order()
        {
            MyTaxi.Models.ViewModels.Order orderData = new Models.ViewModels.Order();

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
                            orderData.isAuthorize = true;
                            orderData.isDriver = true;
                            orderData.userName = dataToDictionaryDriver[0].DriverName;
                            orderData.userSurname = dataToDictionaryDriver[0].DriverSurname;
                            orderData.userPatronymic = dataToDictionaryDriver[0].DriverPatronymic;
                        }
                    }
                    else
                    {
                        dataToDictionaryClient = context.Clients.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).ToList();
                        if (dataToDictionaryClient.Count() == 1)
                        {
                            orderData.isAuthorize = true;
                            orderData.isDriver = false;
                            orderData.userName = dataToDictionaryClient[0].ClientName;
                            orderData.userSurname = dataToDictionaryClient[0].ClientSurname;
                            orderData.userPatronymic = dataToDictionaryClient[0].ClientPatronymic;
                        }
                    }

                    orderData.carClassesToPage = context.CarClasses.ToList();
                }
            }
            else
            {
                orderData.isAuthorize = false;
            }
            #endregion

            return View(orderData);
        }

        [HttpPost]
        public JsonResult GetBaseSumm(string name)
        {
            string infoMessage = null;
            using (var context = new MyTaxiDbContext())
            {
                var findResult = context.CarClasses.Where(cc => cc.CarClassName == name).ToList();
                if (findResult.Count() == 1)
                {
                    infoMessage = findResult[0].CarClassBaseTariff.ToString();
                }
                else
                {
                    infoMessage = "Error";
                }
            }

            return Json(new { resultValue = infoMessage});
        }
    }
}
