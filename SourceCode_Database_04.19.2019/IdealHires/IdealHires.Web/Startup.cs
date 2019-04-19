using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdealHires.Web.Startup))]
namespace IdealHires.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
