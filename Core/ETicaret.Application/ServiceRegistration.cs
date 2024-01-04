using MediatR;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application
{
    public static class ServiceRegistration
    {
        public static void AddAplicationServices(this IServiceCollection service)
        {
            
            service.AddMediatR(typeof(ServiceRegistration));
        } 
    }
}
