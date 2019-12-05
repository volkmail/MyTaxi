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
                    int currentOrderID = -1;

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

                            if (queryHistory.Max(qh => qh.StatusID) == 7)
                            {
                                continue;
                            }

                            List<Tuple<int, string>> statusesCurrentOrder = new List<Tuple<int, string>>();

                            foreach (var st in queryHistory)
                            {
                                statusesCurrentOrder.Add(new Tuple<int, string>(st.StatusID,
                                    context.Statuses.Where(s => s.StatusID == st.StatusID).FirstOrDefault().StatusName));
                            }

                            var queryDriver = context.Drivers.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).FirstOrDefault();

                            //Ограничиваем водителя одним заказом, пока он не завершиться
                            if (allOrders[i].DriverID != null && allOrders[i].DriverID == queryDriver.DriverID)
                            {
                                currentOrderID = allOrders[i].OrderID;
                                tripInfo.Clear();
                                tripInfo.Add(new TripInfo
                                {
                                    OrderID = allOrders[i].OrderID.ToString(),
                                    Addresses = adrNames,
                                    MainSumm = allOrders[i].OrderSum.ToString()
                                });
                                ViewData["Continue"] = "Вернуться к поездке";
                                ViewBag.InfoTrip = tripInfo;
                                return View(allTrips);
                            }

                            // Если заказ закрпелен за водителем, то для других его не выводим
                            if (allOrders[i].DriverID != null && allOrders[i].DriverID != queryDriver.DriverID)
                            {
                                continue; 
                            }

                            tripInfo.Add(new TripInfo
                            {
                                OrderID = allOrders[i].OrderID.ToString(),
                                Addresses = adrNames,
                                MainSumm = allOrders[i].OrderSum.ToString()
                            });
                        }
                        tripInfo.Reverse();
                        if (currentOrderID != -1)
                        {
                            tripInfo = tripInfo.Where(ti => int.Parse(ti.OrderID) == currentOrderID).ToList();
                        }
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

        public IActionResult CurrentTrip(int id) // id - OrderID
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

                    var currentOrder = context.Orders.Where(o => o.OrderID == id).FirstOrDefault();

                    if (currentOrder != null)
                    {
                        TripInfo tripInfo;

                        var queryRoutes = context.Routes.Where(r => r.OrderID == currentOrder.OrderID).ToList();
                        List<string> adrNames = new List<string>();

                        foreach (var adr in queryRoutes)
                        {
                            if (adr.Address != null)
                            {
                                adrNames.Add(adr.Address);
                            }
                        }

                        context.History.Add(new Models.History
                        {
                            OrderID = id,
                            StatusID = 2,
                            HistoryDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm")
                        });

                        if (currentOrder.DriverID == null)
                        {
                            var queryDriver = context.Drivers.Where(d => d.UserID == HttpContext.Session.GetInt32("userID")).FirstOrDefault();
                            currentOrder.DriverID = queryDriver.DriverID;
                        }

                        context.SaveChanges();
                        //TODO: Убрать кнопку "Взять поездку" при наличии 7 статуса

                        var maxStatusForOrder = context.History.Where(h => h.OrderID == currentOrder.OrderID).Max(h => h.StatusID);
                        var statusesCurrentOrder = context.Statuses.Where(s => s.StatusID > maxStatusForOrder).ToArray();

                        tripInfo = new TripInfo
                        {
                            OrderID = currentOrder.OrderID.ToString(),
                            Addresses = adrNames,
                            MainSumm = currentOrder.OrderSum.ToString(),
                        };

                        string statusesForCookie = String.Empty;
                        foreach (var s in statusesCurrentOrder)
                        {
                            statusesForCookie = statusesForCookie + s.StatusID.ToString() + "0" + s.StatusName + "1";
                        }

                        statusesForCookie = statusesForCookie.Remove(statusesForCookie.Length - 1).ToString();

                        HttpContext.Response.Cookies.Append("Statuses", statusesForCookie);
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

        [HttpPost]
        public string SetStatus(int statusId, int orderId)
        {
            string result = String.Empty;

            if (statusId.ToString() != null)
            {
                using (var context = new MyTaxiDbContext())
                {
                    context.History.Add(new Models.History
                    {
                        OrderID = orderId,
                        StatusID = statusId,
                        HistoryDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm")
                    });

                    context.SaveChanges();

                    result = "Success";
                }
            }
            else
                result = "NullId";
            return result;
        }
    }
}
