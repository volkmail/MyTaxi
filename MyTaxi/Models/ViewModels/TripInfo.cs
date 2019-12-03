using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public class TripInfo
    {
        public string OrderID { get; set; }
        public List<string> Addresses { get; set; }
        public string MainSumm { get; set; }
        //public string[] Statuses { get; set; }
    }
}
