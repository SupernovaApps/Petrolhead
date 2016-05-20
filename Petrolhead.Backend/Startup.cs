using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Petrolhead.Backend.Startup))]

namespace Petrolhead.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}