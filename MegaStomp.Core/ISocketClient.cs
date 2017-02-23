using System;
using System.Threading.Tasks;
using MegaStomp.Core.Frame;

namespace MegaStomp.Core
{
    public interface ISocketClient: IDisposable
    {
        Task SendAsync(IFrame frame);

        Task CloseAsync();
    }
}
