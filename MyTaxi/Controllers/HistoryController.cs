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

                    #region FillMainInfo
                    var allOrders = context.Orders.Where(o => o.ClientID == HttpContext.Session.GetInt32("userID")).ToList();
                    if (allOrders.Count > 0)
                    {
                        List<HistoryInfo> historyInfo = new List<HistoryInfo>(allOrders.Count());
                        for (int i = 0; i < allOrders.Count; i++)
                        {
                            var findAdr = context.Routes.Where(r => r.OrderID == allOrders[i].OrderID).ToList();
                            List<String> adrNames = new List<string>();

                            var queryHistory = context.History.Where(h => h.OrderID == allOrders[i].OrderID).ToList();
                            List<String> statuses = new List<string>();
                            List<int> statusesId = new List<int>();
                            bool DriverF = false;

                            foreach (var st in queryHistory)
                            {
                                statusesId.Add(st.StatusID);
                                statuses.Add(context.Statuses.Where(s => s.StatusID == st.StatusID).FirstOrDefault().StatusName);
                            }

                            foreach (var si in statusesId)
                            {
                                if (si != 1)
                                {
                                    DriverF = true;
                                    break;
                                }
                            }

                            foreach (var adr in findAdr)
                            {
                                adrNames.Add(adr.Address);
                            }

                            if (DriverF)
                            {
                                var queryForDrivers = context.Orders.Where(o => o.OrderID == allOrders[i].OrderID).FirstOrDefault().DriverID;
                                var queryDriver = context.Drivers.Where(d => d.DriverID == queryForDrivers).ToList();
                                string tempDriverName = String.Empty;
                                string tempDriverSurname = String.Empty;
                                string tempDriverPatronymic = String.Empty;

                                var queryCarID = context.Drivers.Where(d => d.DriverID == queryForDrivers).FirstOrDefault().CarID;
                                var queryCar = context.Cars.Where(c => c.CarId == queryCarID).FirstOrDefault();
                                var queryCarColor = context.CarColors.Where(cc => cc.CarColorId == queryCar.CarColorId).FirstOrDefault();

                                if (queryDriver.Count == 1)
                                {
                                    tempDriverName = queryDriver[0].DriverName;
                                    tempDriverSurname = queryDriver[0].DriverSurname;
                                    tempDriverPatronymic = queryDriver[0].DriverPatronymic;
                                }
                                else
                                {
                                    tempDriverName = null;
                                    tempDriverSurname = null;
                                    tempDriverPatronymic = null;
                                }

                                historyInfo.Add(new HistoryInfo
                                {
                                    OrderID = allOrders[i].OrderID.ToString(),
                                    Addresses = adrNames,
                                    Statuses = statuses,
                                    DriverFound = true,
                                    DriverName = tempDriverName,
                                    DriverSurname = tempDriverSurname,
                                    DriverPatronymic = tempDriverPatronymic,
                                    CarNumber = queryCar.CarNumber,
                                    CarMark = queryCar.CarMark,
                                    CarModel = queryCar.CarModl,
                                    CarColor = queryCarColor.CarColorName,
                                    MainSumm = context.Orders.Where(o => o.OrderID == allOrders[i].OrderID).FirstOrDefault().OrderSum.ToString()
                                });
                            }
                            else
                            {
                                historyInfo.Add(new HistoryInfo
                                {
                                    OrderID = allOrders[i].OrderID.ToString(),
                                    Addresses = adrNames,
                                    Statuses = statuses,
                                    DriverFound = false,
                                    DriverName = null,
                                    DriverSurname = null,
                                    DriverPatronymic = null,
                                    MainSumm = context.Orders.Where(o => o.OrderID == allOrders[i].OrderID).FirstOrDefault().OrderSum.ToString()
                                });
                            }
                        }
                        ViewBag.Info = historyInfo;
                        return View(history);
                        #endregion
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
