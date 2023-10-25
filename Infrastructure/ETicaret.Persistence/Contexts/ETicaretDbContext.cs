using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Contexts
{
    public class ETicaretDbContext : DbContext
    {
        public ETicaretDbContext(DbContextOptions<ETicaretDbContext> options) : base(options)
        {}
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Customer> Customers{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o=>o.Customer).WithMany(c=>c.Orders).HasForeignKey(o=>o.CustomerId);
            });
        }

    }
}
