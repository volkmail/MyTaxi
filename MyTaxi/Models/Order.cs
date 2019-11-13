using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int DriverID { get; set; }
        public Driver Driver { get; set; }
        public int ClientID {get;set;}
        public Client Client { get; set; }
        public int OrderSum { get; set; }
        public ICollection<History> History { get; set; }
        public ICollection<Route> Routes { get; set; }

        public Order()
        {
            History = new List<History>();
            Routes = new List<Route>();
        }
    }
}
