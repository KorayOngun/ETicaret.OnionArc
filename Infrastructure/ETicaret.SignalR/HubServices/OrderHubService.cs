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
    public class OrderHubService : IOrderHubService
    {
        private IHubContext<OrderHub> _hubcontext;

        public OrderHubService(IHubContext<OrderHub> hubcontext)
        {
            _hubcontext = hubcontext;
        }

        public async Task OrderAddedMessageAsync(string message)
        {
            await _hubcontext.Clients.All.SendAsync(ReceiveFunctionsNames.OrderAddedMessage, message);
            // TODO All yerine Other kullan
        }
    }
}
