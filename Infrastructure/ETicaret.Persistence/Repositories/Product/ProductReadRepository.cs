﻿using ETicaret.Application.Repositories;
using ETicaret.Domain.Entities;
using ETicaret.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Repositories
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        public ProductReadRepository(ETicaretDbContext context) : base(context)
        {
          
        }
    }
}
