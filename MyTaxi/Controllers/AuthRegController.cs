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
    public class AuthRegController : Controller
    {
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorization(string login, string password)
        {
            using (var context = new MyTaxiDbContext())
            {
                if (context.Users.Any())
                {
                    var compare_result = context.Users.Where(u => u.UserLogin == login && u.UserPassword == password).ToList();

                    if (compare_result.Count() == 1)
                    {
                        HttpContext.Session.SetInt32("userID", compare_result[0].UserID);
                        HttpContext.Session.SetInt32("isDriver", compare_result[0].DriverSign ? 1 : 0);

                        return Redirect("/Home/Index");
                    }
                    else
                        return View();
                }
                return View();
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(ClientRegistration clientRegistration)
        {
            if (clientRegistration != null)
            {
                using (var context = new MyTaxiDbContext())
                {
                    context.Users.Add(new User
                    {
                        UserLogin = clientRegistration.login,
                        UserPassword = clientRegistration.password,
                        DriverSign = false
                    });

                    context.SaveChanges();

                    context.Clients.Add(new Client { 
                        ClientName = clientRegistration.userName,
                        ClientSurname = clientRegistration.userSurname,
                        ClientPatronymic = clientRegistration.userPatronymic,
                        ClientPhoneNumber = clientRegistration.userPhone,
                        UserID = context.Users.OrderBy(us => us.UserID).Last().UserID
                    });

                    context.SaveChanges();

                    ClientRegistrationInit cri = new ClientRegistrationInit();
                    cri.Success = true;
                    cri.JustInit = false;
                    return View(cri);
                }
            }
            else
            {
                return Redirect("/AuthReg/Authorization");
            }
        }

        public IActionResult RegistrationWay()
        {
            return View();
        }

        public IActionResult RegistrationDriver()
        {
            using (var context = new MyTaxiDbContext())
            {
                DriverRegistrationInit dri = new DriverRegistrationInit();

                dri.CarClasses = context.CarClasses.ToList();
                dri.CarColors = context.CarColors.ToList();
                dri.Success = false;
                dri.JustInit = true;

                return View(dri);
            }
        }

        [HttpPost]
        public IActionResult RegistrationDriver(DriverRegistration driverRegistration)
        {
            if (driverRegistration != null)
            {
                using (var context = new MyTaxiDbContext())
                {
                    context.Users.Add(new User
                    {
                        UserLogin = driverRegistration.login,
                        UserPassword = driverRegistration.password,
                        DriverSign = false
                    });

                    context.Cars.Add(new Car
                    {
                        CarNumber = driverRegistration.carNumber,
                        CarMark = driverRegistration.carMark,
                        CarModl = driverRegistration.carModel,
                        CarClassId = context.CarClasses.Where(cc => cc.CarClassName == driverRegistration.carClass)
                                                       .FirstOrDefault()?.CarClassId,
                        CarColorId = context.CarColors.Where(cc => cc.CarColorName == driverRegistration.carColor)
                                                       .FirstOrDefault()?.CarColorId
                    });

                    context.SaveChanges();

                    context.Drivers.Add(new Driver
                    {
                        DriverName = driverRegistration.driverName,
                        DriverSurname = driverRegistration.driverSurname,
                        DriverPatronymic = driverRegistration.driverPatronymic,
                        DriverPhoneNumber = driverRegistration.driverPhone,
                        UserID = context.Users.Local.Max(us => us.UserID),
                        CarID = context.Cars.Local.Max(car => car.CarId)
                    });

                    context.SaveChanges();

                    DriverRegistrationInit dri = new DriverRegistrationInit();

                    dri.CarClasses = context.CarClasses.ToList();
                    dri.CarColors = context.CarColors.ToList();
                    dri.Success = true;
                    dri.JustInit = false;

                    return View(dri);
                }
            }
            else
            {
                //TODO: AuthRegController вывод ошибки о том, что не введены никакие данные водителя
                using (var context = new MyTaxiDbContext())
                {
                    DriverRegistrationInit dri = new DriverRegistrationInit();

                    dri.CarClasses = context.CarClasses.ToList();
                    dri.CarColors = context.CarColors.ToList();
                    dri.Success = false;
                    dri.JustInit = true;

                    return View(dri);
                }
            }
        }

    }
}
