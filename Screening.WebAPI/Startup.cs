using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Screening.WebAPI.Startup))]

namespace Screening.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
