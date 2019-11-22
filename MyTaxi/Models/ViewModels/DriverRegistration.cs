using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public class DriverRegistration
    {
        public string login { get; set; }
        public string password { get; set; }
        public string driverName { get; set; }
        public string driverSurname { get; set; }
        public string driverPatronymic { get; set; }
        public string driverPhone { get; set; }
        public string carNumber { get; set; }
        public string carModel { get; set; }
        public string carMark { get; set; }
        public string carClass { get; set; }
        public string carColor { get; set; }
    }
}
