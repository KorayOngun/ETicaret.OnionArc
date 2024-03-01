using ETicaret.Application.Abstractions.Hubs;
using ETicaret.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        readonly IHubContext<ProductHub> _context;
        
        public ProductHubService(IHubContext<ProductHub> context)
        {
            _context = context;
        }

        public async Task ProductAddedMessageAsync(string message)
        {
            await _context.Clients.All.SendAsync(ReceiveFunctionsNames.ProductAddedMessage,message);
        }
    }
}
