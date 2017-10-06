using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalrAngular.Hubs
{
    public class ChatHub : Hub
    {
        public Task Send(string data)
        {
            var currentUser = new List<string> { Context.ConnectionId };
            return Clients.AllExcept(currentUser).InvokeAsync("Send", data);
        }
    }
}