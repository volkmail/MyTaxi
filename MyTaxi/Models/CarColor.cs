using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyTaxi.Models
{
    public class CarColor
    {
        [Key]
        public int CarColorId { get; set; }

        public string CarColorName { get; set; }

        public ICollection<Car> Cars { get; set; }

        public CarColor()
        {
            Cars = new List<Car>();
        }
    }
}
