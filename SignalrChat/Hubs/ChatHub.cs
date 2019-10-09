using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngularCore
{
    public class ChatHub : Hub
    {
        public Task SendGood(string data)
        {
            var currentUser = new List<string> {Context.ConnectionId};
            return Clients.AllExcept(currentUser).SendAsync("SendGood", data);
        }

        public Task SendBad(string data)
        {
            var currentUser = new List<string> { Context.ConnectionId };
            return Clients.AllExcept(currentUser).SendAsync("SendBad", data);
        }

    }
}