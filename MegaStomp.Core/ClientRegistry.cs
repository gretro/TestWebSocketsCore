using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaStomp.Core
{
    public class ClientRegistry: IDisposable
    {
        public static ClientRegistry Instance { get; private set; } = new ClientRegistry();

        public List<ISocketClient> Clients { get; } = new List<ISocketClient>();

        private ClientRegistry()
        {
            
        }

        public void RegisterClient(ISocketClient client)
        {
            Clients.Add(client);
        }

        public Task CloseAllAsync()
        {
            var closeTask = Task.WhenAll(Clients.Select(client => client.CloseAsync()));
            return closeTask;
        }

        public void Dispose()
        {
            CloseAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Instance = new ClientRegistry();
        }
    }
}
