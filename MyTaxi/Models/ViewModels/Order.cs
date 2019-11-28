using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public class Order : UserData
    {
        public List<CarClass> carClassesToPage { get; set; }
    }
}
