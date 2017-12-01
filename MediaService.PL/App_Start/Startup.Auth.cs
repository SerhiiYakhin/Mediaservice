#region usings

using MediaService.BLL.Interfaces;
using MediaService.PL.Models.IdentityModels;
using MediaService.PL.Models.IdentityModels.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Web.Mvc;

#endregion

namespace MediaService.PL
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {

            app.CreatePerOwinContext(() => (IUserService) DependencyResolver.Current.GetService(typeof(IUserService)));
            app.CreatePerOwinContext(() =>
                (IFileService) DependencyResolver.Current.GetService(typeof(IFileService)));
            app.CreatePerOwinContext(() =>
                (IDirectoryService) DependencyResolver.Current.GetService(typeof(IDirectoryService)));
            app.CreatePerOwinContext(() =>
                (IUserProfileService) DependencyResolver.Current.GetService(typeof(IUserProfileService)));
            app.CreatePerOwinContext(() =>
                (ITagService)DependencyResolver.Current.GetService(typeof(ITagService)));

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            TimeSpan.FromMinutes(30),
                            (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseFacebookAuthentication(
                "1956709024594856",
                "4fd4b9b1f37c56db5ca44d8318d48ffa");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "399844496480-amechr0p04vqj11hv17qvfocs2lf9428.apps.googleusercontent.com",
                ClientSecret = "4XJTMEEOSvWoTli2X2tZ8Cq3"
            });
        }
    }
}