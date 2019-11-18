using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTaxi.Models
{ 
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        [MaxLength(20)]
        public string ClientName { get; set; }
        [MaxLength(20)]
        public string ClientSurname { get; set; }
        [MaxLength(20)]
        public string ClientPatronymic { get; set; }
        [MaxLength(20)]
        public string ClientPhoneNumber { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Client() => Orders = new List<Order>();
    }
}
