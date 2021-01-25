using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class GeneralStoreDbContext : DbContext
    {
            public GeneralStoreDbContext()
                : base("DefaultConnection")
            {

            }
            public DbSet<Customer> Customer { get; set; }
            public DbSet<Product> Product { get; set; }
            public DbSet<Transaction> Transaction { get; set; }

    }
}   