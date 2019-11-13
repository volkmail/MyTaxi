﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyTaxi.Models
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }
        [MaxLength(20)]
        public string DriverName { get; set; }
        [MaxLength(20)]
        public string DriverSurname { get; set; }
        [MaxLength(20)]
        public string DriverPatronymic { get; set; }
        [MaxLength(20)]
        public string DriverPhoneNumber { get; set; }
        public int CarID { get; set; }
        public Car Car { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Driver() => Orders = new List<Order>();
    }
}
