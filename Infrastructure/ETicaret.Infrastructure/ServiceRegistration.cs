using ETicaret.Application.Abstractions.Storage;
using ETicaret.Application.Abstractions.Storage.Local;
using ETicaret.Application.Repositories;
using ETicaret.Infrastructure.Enums;
using ETicaret.Infrastructure.Services;
using ETicaret.Infrastructure.Services.Storage;
using ETicaret.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
        }
        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }
      
    }
}
