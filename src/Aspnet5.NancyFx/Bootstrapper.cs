using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using WebApplication4.Models;

namespace WebApplication4
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
           
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
           
            var usermanager = new UserManager<User>(new MongoDB.AspNet.Identity.UserStore<User>(config["Data:Default:ConnectionString"]));
            usermanager.UserValidator = new UserValidator<User>(usermanager) { AllowOnlyAlphanumericUserNames = false };
            container.Register(usermanager);
            container.Register(config);
        }
    }
}