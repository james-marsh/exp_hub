using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EXPLOSION_HUB.Startup))]
namespace EXPLOSION_HUB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
