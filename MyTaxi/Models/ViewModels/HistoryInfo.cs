using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaxi.Models.ViewModels
{
    public class HistoryInfo
    {
        public string OrderID { get; set; }
        public List<String> Addresses { get; set; }
        public List<String> Statuses { get; set; }
        public bool DriverFound { get; set; } = false;
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string DriverPatronymic { get; set; }
        public string CarNumber { get; set; }
        public string CarModel { get; set; }
        public string CarMark { get; set; }
        public string CarColor { get; set; }
        public string MainSumm { get; set; }
    }
}
