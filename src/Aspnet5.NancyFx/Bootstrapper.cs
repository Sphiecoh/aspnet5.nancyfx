using AspNet.Identity.MongoDB;
using Aspnet5.NancyFx.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
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
            container.Register(config);
            SetupMembership(container);
           
        }

        private void SetupMembership(TinyIoCContainer container)
        {
            var settings = container.Resolve<IConfigurationRoot>().Get<DbSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.Database);
            var users = db.GetCollection<User>(settings.UserTable);
            var roles = db.GetCollection<IdentityRole>(settings.RoleTable);
            var store = new UserStore<User>(new UsersContext<User>(users));

            var usermanager = new UserManager<User>(store);
            usermanager.UserTokenProvider = new TotpSecurityStampBasedTokenProvider<User, string>() {};
            usermanager.UserLockoutEnabledByDefault = true;
            usermanager.MaxFailedAccessAttemptsBeforeLockout = 3;
            usermanager.UserValidator = new UserValidator<User>(usermanager) { AllowOnlyAlphanumericUserNames = false };
          
            var roleStore = new RoleStore<IdentityRole>(new RolesContext<IdentityRole>(roles));
            IndexChecks.EnsureUniqueIndexOnEmail(users);
            IndexChecks.EnsureUniqueIndexOnRoleName(roles);
            IndexChecks.EnsureUniqueIndexOnUserName(users);
            container.Register(usermanager);

        }
    }
}