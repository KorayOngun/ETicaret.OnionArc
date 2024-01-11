using ETicaret.Domain.Entities;
using ETicaret.Domain.Entities.Common;
using ETicaret.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Contexts
{
    public class ETicaretDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        
        public ETicaretDbContext(DbContextOptions<ETicaretDbContext> options) : base(options)
        {}
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Customer> Customers{ get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           foreach(var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    //case EntityState.Detached:
                    //    break;
                    //case EntityState.Unchanged:
                    //    break;
                    //case EntityState.Deleted:
                    //    break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate= DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o=>o.Customer).WithMany(c=>c.Orders).HasForeignKey(o=>o.CustomerId);
            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
