using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Budget.WebMVC.Startup))]
namespace Budget.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
