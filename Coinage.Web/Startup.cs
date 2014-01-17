using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Coinage.Web.Startup))]
namespace Coinage.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
