using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Screening.WebAPI.Models;
using Screening.WebAPI.Providers;
using System.Web.Http;
using Unity.WebApi;

namespace Screening.WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IOAuthAuthorizationServerProvider, ApplicationOAuthProvider>();
            container.RegisterType<IUserStore<ApplicationUser>, FakeApplicationUserStore>();
            container.RegisterType<ISecureDataFormat<AuthenticationTicket>>(new InjectionFactory((c) => null));
            container.RegisterType<UserManager<ApplicationUser>, ApplicationUserManager>();
            var resolver = new UnityDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            
        }
    }
}