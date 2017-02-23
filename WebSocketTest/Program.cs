using System.IO;
using MegaStomp.Core;
using Microsoft.AspNetCore.Hosting;

namespace WebSocketTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();

            ClientRegistry.Instance.Dispose();
        }
    }
}
