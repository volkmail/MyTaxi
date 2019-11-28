using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public class DriverRegistrationInit : UserData
    {
        public List<CarClass> CarClasses { get; set; }
        public List<CarColor> CarColors { get; set; }
        public bool Success { get; set; }
        public bool JustInit { get; set; }
    }
}
