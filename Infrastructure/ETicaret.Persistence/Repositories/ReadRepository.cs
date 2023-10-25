using ETicaret.Application.Repositories;
using ETicaret.Domain.Entities.Common;
using ETicaret.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ETicaretDbContext _context;
        public ReadRepository(ETicaretDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) return query.AsNoTracking();
            return query;
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            //return await Table.FirstOrDefaultAsync(x=>x.Id== Guid.Parse(id));
            return (tracking == true ? await Table.FirstOrDefaultAsync(x=>x.Id==Guid.Parse(id)) : await Table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id)));


        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
           return  (tracking == true ? await Table.FirstOrDefaultAsync(method) : await Table.AsNoTracking().FirstOrDefaultAsync(method));
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter, bool tracking = true)
        {
           var query = Table.Where(filter);
            if (!tracking) return query.AsNoTracking();
            return query;
        }
    }
}
