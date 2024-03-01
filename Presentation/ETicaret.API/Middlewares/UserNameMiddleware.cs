
using Serilog.Context;

namespace ETicaret.API.Middlewares
{
    public class UserNameMiddleware : IMiddleware
    {
        public UserNameMiddleware()
        {
            
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
            LogContext.PushProperty("user_name", userName);
            await next.Invoke(context);
        }
    }
}
