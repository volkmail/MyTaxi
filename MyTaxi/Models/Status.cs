using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{
    public class Status
    {
        [Key]
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public ICollection<History> History { get; set; }

        public Status() => History = new List<History>();
    }
}
