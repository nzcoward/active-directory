using System;
using System.Reflection;
using System.Web.Http;
using Arc.ActiveDirectory.Api;
using Arc.ActiveDirectory.Shared;
using Arc.Authentication.Policy;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using Arc.ActiveDirectory.Shared.Errors;
using System.Configuration;

[assembly: OwinStartup(typeof(Startup))]
namespace Arc.ActiveDirectory.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var policy = new LockoutPolicy(new[] {
                new LockoutPolicyItem(x => x == 1 || x == 2, "Login unsuccessful"),
                new LockoutPolicyItem(x => x == 3 || x == 4, "Login unsuccessful. You will now need to wait 30 seconds before you can attempt to log in again.", TimeSpan.FromSeconds(30)),
                new LockoutPolicyItem(x => x == 5 || x == 6, "Login unsuccessful. You will now need to wait 2 minutes before you can attempt to log in again.", TimeSpan.FromMinutes(2)),
                new LockoutPolicyItem(x => x == 7 || x == 8 || x == 9, "Login unsuccessful. You will now need to wait 30 minutes before you can attempt to log in again.", TimeSpan.FromMinutes(30)),
                new LockoutPolicyItem(x => x >= 10, "Login unsuccessful. You will now need to wait 24 hours before you can attempt to log in again.", TimeSpan.FromHours(24))
            });

            var errors = (ErrorMessagesSection)ConfigurationManager.GetSection("tlc.errors");
            builder.RegisterInstance(new ErrorMessages(errors.ErrorMessages));

            builder.RegisterInstance(new FailedLoginManager(policy));

            builder.RegisterType<WebRequestsLogger>().InstancePerRequest();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);


            //if (Constants.EncryptConfig)
            //{
            //    app.UseConfigEncryptionOnStartUp("appSettings");
            //}

            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}