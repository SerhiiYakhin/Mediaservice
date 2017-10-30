using Microsoft.Owin;
using MS.BusinessLayer.Interfaces;
using Owin;

[assembly: OwinStartup(typeof(MediaService.Startup))]
namespace MediaService
{
    public partial class Startup
    {
        // add this static variable
        internal static string AppName { get; private set; }
        private readonly IUserService _userService;

        public Startup(IUserService userService)
        {
            _userService = userService;
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //todo: find how it works and make it work right
            AppName = app.Properties["Owin.AppName"] as string ?? "MediaService";
        }
    }
}
