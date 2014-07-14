using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcDrag.Startup))]
namespace MvcDrag
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
