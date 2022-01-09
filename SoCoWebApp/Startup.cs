using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoCoWebApp.Startup))]
namespace SoCoWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
