using Microsoft.AspNetCore.SignalR;

namespace MvcDrag.Controllers
{
    public class DragHub : Hub
    {
        public void ItemDragged(int x, int y)
        {
            Clients.Others.SendAsync("onDrag", new { X = x, Y = y });
        }
    }
}