using ETicaret.Application.Abstractions.Hubs;
using ETicaret.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection service)
        {
            service.AddTransient<IProductHubService, ProductHubService>();
            service.AddTransient<IOrderHubService, OrderHubService>();
            service.AddSignalR();
        }
         
    }
}
