using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.ModelBinding;
using System.Threading;
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
            Post["/confirm/{user}/{token}"] = (paramz,token) => ConfirmEmail(paramz);
            Get["/{user}/token"] = (paramz,token) => GenerateToken(paramz);
        }

        private async Task<dynamic> GenerateToken(dynamic paramz)
        {
            return Response.AsJson(await _userManager.GenerateEmailConfirmationTokenAsync((string)paramz.user));
        }

        private async Task<dynamic> ConfirmEmail(dynamic paramz)
        {
            var token = paramz.token;
            var id = paramz.user;
            IdentityResult result =await _userManager.ConfirmEmailAsync(id, token);
            return Negotiate.WithModel(result);

        }

        private async Task<dynamic> CreateUser(dynamic arg , CancellationToken token)
        {
            var dto = this.Bind<UserDto>();
            var user = new User { FullName = dto.FullName,UserName = dto.EmailAddress };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if(result.Succeeded)
            {
                await _userManager.SetEmailAsync(user.Id, dto.EmailAddress);
                var _token =await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                return Negotiate.WithModel(new { _token, user.Id });
            }
            return Negotiate.WithModel(result);

        }
    }
}
