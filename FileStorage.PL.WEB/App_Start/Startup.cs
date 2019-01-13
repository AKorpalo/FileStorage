using System.Configuration;
using FileStorage.BLL.Interfaces;
using FileStorage.BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(FileStorage.PL.WEB.App_Start.Startup))]

namespace FileStorage.PL.WEB.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();//ninject

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService); // ninject
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService(ConfigurationManager.ConnectionStrings["TestDBConnection"].ConnectionString);
        }
    }
}