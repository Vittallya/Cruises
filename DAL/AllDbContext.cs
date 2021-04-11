using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using DAL.Models;

namespace DAL
{
    public class AllDbContext: DbContext
    {        


        public AllDbContext():base("CruisesDb")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.Profile> Profiles { get; set; }
        public DbSet<Models.Tour> Tours { get; set; }
    }
}
