using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{  
    public class History
    {
        [Key]
        public int HistoryID { get; set; }
        public DateTime HistoryDate { get; set; }
        public int StatusID { get; set; }
        public Status Status { get; set; }
        public int OrderID{get; set;}
        public Order Order { get; set; }
    }
}
