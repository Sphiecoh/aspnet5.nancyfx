using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Modules
{
    public class UserModule : NancyModule
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfigurationRoot _config;
        public UserModule(UserManager<User> userManager, IConfigurationRoot config): base("/user")
        {
            _userManager = userManager;
            _config = config;
            
            Post["/"] = CreateUser;
        }

        private dynamic CreateUser(dynamic arg)
        {
            var dto = this.Bind<UserDto>();
            var user = new User { FullName = dto.FullName, EmailAddress = dto.EmailAddress, UserName = dto.EmailAddress };
            var result = _userManager.Create(user);
            return Negotiate.WithModel(result);

        }
    }
}
