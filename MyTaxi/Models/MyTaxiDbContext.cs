﻿using Microsoft.EntityFrameworkCore;

namespace MyTaxi.Models
{   
    public class MyTaxiDbContext:DbContext
    {
        #region Public Properties
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarClass> CarClasses { get; set; }
        public DbSet<CarColor> CarColors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        public MyTaxiDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=.;Database=MyTaxiDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
