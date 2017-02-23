using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MegaStomp.Core;
using MegaStomp.Core.Frame;

namespace MegaStomp.AspNetCore
{
    public sealed class WebSocketClient: ISocketClient
    {
        public WebSocket WebSocket { get; }

        public WebSocketClient(WebSocket webSocket)
        {
            WebSocket = webSocket;
        }

        public async Task ListenAsync()
        {
            while (WebSocket.State == WebSocketState.Open)
            {
                var cancellationSource = new CancellationTokenSource();

                try
                {
                    var buffer = new byte[8192]; // TODO: Move to config.
                    await WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationSource.Token);

                    var msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                    Debug.WriteLine($"Received message: {msg}");
                }
                catch (TaskCanceledException)
                {
                    Debug.WriteLine($"Error while listening to the WebSocket");   
                }
            }
        }

        public void Dispose()
        {
            WebSocket.Dispose();
        }

        public Task SendAsync(IFrame frame)
        {
            var message = frame.GetMessage();
            var bytes = Encoding.UTF8.GetBytes(message);

            return WebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, default(CancellationToken));
        }

        public Task CloseAsync()
        {
            return WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None);
        }
    }
}
