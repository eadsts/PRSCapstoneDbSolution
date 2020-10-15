using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRSCapstoneDb.Models;

namespace PRSCapstoneDb.Data
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }

        //add this fluent api code to make a column unique in a table/class
        //need to add one for each class - we are doing Customer
        //there is an index on the code column using HasIndex
        //need to append with IsUnique to make Code column unique
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(e =>
            {
                e.HasIndex(p => p.Username).IsUnique();
            });
            builder.Entity<Vendor>(e =>
            {
                e.HasIndex(p => p.Code).IsUnique();
            });
            builder.Entity<Product>(e =>
            {
                e.HasIndex(p => p.PartNbr).IsUnique();
            });
        }
        
    }
}
