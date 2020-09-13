using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RxSignalrSharpWeb.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("addMessage", message);
        }
    }
}
