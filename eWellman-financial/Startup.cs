using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eWellman_financial.Startup))]
namespace eWellman_financial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
