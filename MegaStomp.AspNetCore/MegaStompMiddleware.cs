using System.Threading.Tasks;
using MegaStomp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MegaStomp.AspNetCore
{
    public static class MegaStompMiddlewareHelper
    {
        public static IApplicationBuilder UseMegaStomp(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<MegaStompMiddleware>();
        }
    }

    public class MegaStompMiddleware
    {
        private readonly RequestDelegate _next;

        public MegaStompMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
				var socketClient = new WebSocketClient(webSocket);

				ClientRegistry.Instance.RegisterClient(socketClient);
                await socketClient.ListenAsync();
            }
            else
            {
                await _next(context);
            }
        }
    }
}
