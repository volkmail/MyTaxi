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

            #region Fill User Data & TripsInfo

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

                    #region TripsInfo


                    var allOrders = context.Orders.ToList();
                    
                    if (allOrders.Any())
                    {
                        List<TripInfo> tripInfo = new List<TripInfo>();

                        for (int i = 0; i < allOrders.Count; i++)
                        {
                            var queryRoutes = context.Routes.Where(r => r.OrderID == allOrders[i].OrderID).ToList();
                            List<string> adrNames = new List<string>();

                            foreach (var adr in queryRoutes)
                            {
                                if (adr.Address != null)
                                {
                                    adrNames.Add(adr.Address);
                                }
                            }

                            var queryHistory = context.History.Where(h => h.OrderID == allOrders[i].OrderID).ToList();
                            List <Tuple<int, string>> statusesCurrentOrder = new List<Tuple<int, string>>();

                            foreach (var st in queryHistory)
                            {
                                statusesCurrentOrder.Add(new Tuple<int, string>(st.StatusID,
                                    context.Statuses.Where(s => s.StatusID == st.StatusID).FirstOrDefault().StatusName));
                            }

                            if (statusesCurrentOrder.OrderBy(stco => stco.Item1).Where(stco => stco.Item1 > 1).Any())
                            {
                                continue; // Если есть статус выше чем 1 - "Поиск водителя", то мы не выводим этот заказ для водителя
                            }

                            tripInfo.Add(new TripInfo
                            {
                                OrderID = allOrders[i].OrderID.ToString(),
                                Addresses = adrNames,
                                MainSumm = allOrders[i].OrderSum.ToString()
                            });
                        }
                        tripInfo.Reverse();
                        ViewBag.InfoTrip = tripInfo;
                        return View(allTrips);
                    }

                    #endregion
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
