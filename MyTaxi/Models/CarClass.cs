using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{
    public class CarClass
    {
        [Key]
        public int CarClassId { get; set; }
        [MaxLength(15)]
        public string CarClassName { get; set; }

        public int CarClassBaseTariff { get; set; }

        public ICollection<Car> Cars { get; set; }

        public CarClass()
        {
            Cars = new List<Car>();
        }
    }
}
