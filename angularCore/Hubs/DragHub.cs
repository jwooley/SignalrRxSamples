using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCore
{
    public class DragHub : Hub
    {
        public async Task DragDrop(Coords position)
        {
            await Clients.Others.SendAsync("Dragged", position);
        }
    }
    public class Coords
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
