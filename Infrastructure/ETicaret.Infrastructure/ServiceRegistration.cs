using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Abstractions.Services.Configurations;
using ETicaret.Application.Abstractions.Storage;
using ETicaret.Application.Abstractions.Storage.Local;
using ETicaret.Application.Abstractions.Token;
using ETicaret.Application.Repositories;
using ETicaret.Application.SettingObject;
using ETicaret.Infrastructure.Configuration;
using ETicaret.Infrastructure.Enums;
using ETicaret.Infrastructure.Services;
using ETicaret.Infrastructure.Services.Storage;
using ETicaret.Infrastructure.Services.Storage.Local;
using ETicaret.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {


            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IApplicationService, ApplicationService>();

        }
        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }
      
    }
}
