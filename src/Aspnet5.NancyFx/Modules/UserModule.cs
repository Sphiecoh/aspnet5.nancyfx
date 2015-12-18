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
            Get["/setpassword/{token:string}"] = _ => View["setpassword"];
            Get["/confirm/{user}/{token}"] = paramz => ConfirmEmail(paramz);
            Get["/{user}/token"] = paramz => GenerateToken(paramz);
        }

        private dynamic GenerateToken(dynamic paramz)
        {
            return Response.AsJson(_userManager.GenerateEmailConfirmationToken((string)paramz.user));
        }

        private dynamic ConfirmEmail(dynamic paramz)
        {
            var token = paramz.token;
            var id = paramz.user;
            IdentityResult result = _userManager.ConfirmEmailAsync(id, token).Result;
            return Negotiate.WithModel(result);

        }

        private dynamic CreateUser(dynamic arg)
        {
            var dto = this.Bind<UserDto>();
            var user = new User { FullName = dto.FullName,UserName = dto.EmailAddress };
            var result = _userManager.Create(user);
            if(result.Succeeded)
            {
                _userManager.SetEmail(user.Id, dto.EmailAddress);
                var token = _userManager.GenerateEmailConfirmationToken(user.Id);
                return Negotiate.WithModel(new { token, user.Id });
            }
            return Negotiate.WithModel(result);

        }
    }
}
