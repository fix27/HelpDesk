using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HelpDesk.WorkerWebApp.Startup))]
namespace HelpDesk.WorkerWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
