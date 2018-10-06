using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngularCore
{
    public class ChatHub : Hub
    {
        public async Task Send(string data)
        {
            var currentUser = new List<string> {Context.ConnectionId};
            await Clients.AllExcept(currentUser).SendAsync("Send", data);
        }
    }
}