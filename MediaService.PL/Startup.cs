using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MediaService.PL.Startup))]
namespace MediaService.PL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureNinject();
            ConfigureAuth(app);
        }
    }
}
