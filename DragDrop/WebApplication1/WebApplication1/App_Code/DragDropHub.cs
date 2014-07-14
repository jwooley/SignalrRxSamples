using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication1.App_Code
{
    [HubName("dragDrop")]
    public class DragDropHub : Hub
    {
        public void MoveShape(int x, int y)
        {
            Clients.Others.shapeMoved(x, y);
        }
    }
}