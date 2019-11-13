using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{
    public class Route
    {
        [Key]
        public int RouteID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public string Address { get; set; }
    }
}
