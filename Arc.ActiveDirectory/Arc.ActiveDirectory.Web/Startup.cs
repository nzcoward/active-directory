using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Arc.ActiveDirectory.Web;
using Arc.ConfigurationEncryption;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using Arc.ActiveDirectory.Shared.Errors;
using System.Configuration;

[assembly: OwinStartup(typeof(Startup))]
namespace Arc.ActiveDirectory.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //app.Use<WebRequestsLogger>();
            if (Constants.EncryptConfig)
            {
                app.UseConfigEncryptionOnStartUp("appSettings");
            }

            var builder = new ContainerBuilder();
            
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<PasswordPolicy>().As<PasswordPolicy>();
            builder.RegisterType<UrlValidator>().As<UrlValidator>();
            builder.RegisterType<TokenValidator>().As<TokenValidator>();

            var errors = (ErrorMessagesSection)ConfigurationManager.GetSection("tlc.errors");
            builder.RegisterInstance(new ErrorMessages(errors.ErrorMessages));

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}