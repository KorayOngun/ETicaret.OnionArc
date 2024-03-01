
using ETicaret.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace ETicaret.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication app)
        {
            app.MapHub<ProductHub>("/products-hub");
            app.MapHub<OrderHub>("/orders-hub");
        }
    }
}
