using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MegaStomp.AspNetCore;
using MegaStomp.Core;
using MegaStomp.Core.Frame;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebSocketTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.UseMegaStomp();

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Path == "/pong")
                {
                    var tasks =
                        ClientRegistry.Instance.Clients.Select(client => client.SendAsync(new BasicFrame("PONG!")));
                    await Task.WhenAll(tasks);

                    await ctx.Response.WriteAsync("PONG! Sent to everyone.");
                }
                else
                {
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            
        }

        private Task SendThroughWebSocket(string msg, WebSocket ws)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            return ws.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
