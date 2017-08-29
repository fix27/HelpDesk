using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HelpDesk.CabinetWebApp.Startup))]
namespace HelpDesk.CabinetWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
