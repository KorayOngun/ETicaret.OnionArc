using ETicaret.Application.Repositories;
using ETicaret.Domain.Entities.Common;
using ETicaret.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        readonly private ETicaretDbContext _context;

        public WriteRepository(ETicaretDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T entity)
        {
           EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> entity)
        {
           await  Table.AddRangeAsync(entity);
            return true;
        }
        public bool Remove(T entity)
        {
            var entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> Remove(string id)
        {
            var model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return Remove(model);
        }

        public bool RemoveRange(List<T> entities)
        {
            Table.RemoveRange(entities);
            return true;
        }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public bool Update(T entity)
        {
           var entityState =   Table.Update(entity);
           return entityState.State == EntityState.Modified;
        }
    }
}
