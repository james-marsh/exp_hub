using EXPLOSION_HUB.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Startup))]
namespace EXPLOSION_HUB
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            App_Start.Startup.ConfigureAuth(app);
        }
    }
}
