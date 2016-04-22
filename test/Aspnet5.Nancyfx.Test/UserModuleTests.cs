using GenFu;
using Nancy;
using Nancy.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4;
using WebApplication4.Models;
using Xunit;

namespace Aspnet5.Nancyfx.Test
{
    public class UserModuleTests
    {
        private readonly Browser browser;
        public UserModuleTests()
        {
            browser = new Browser(new Bootstrapper(), with => { with.Accept("application/json"); });
            GenFu.GenFu.Configure<UserDto>().Fill(x => x.Password).WithRandom(new[] { "1236547", "theisnorole", "executive" }).Fill(x => x.EmailAddress).AsEmailAddress().Fill(x => x.FullName).AsMusicArtistName();
        }

        [Fact]
        public async void Should_Create_User()
        {

            var user = A.New<UserDto>();
            var response = await browser.Post("/user", w => w.JsonBody(user));
            var result = response.Body.DeserializeJson<dynamic>();
            Assert.True(((string)result.Token).Length > 0);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void Should_NotCreate_User_WhenNoEmailAddress()
        {
             
            var user = new UserDto { Password = string.Empty};
            var response = await browser.Post("/user", w => w.JsonBody(user));
           Assert.True(response.StatusCode == HttpStatusCode.UnprocessableEntity);
        }
        [Fact]
        public async void Should_Confirm_UserEmail()
        {
           
            var user = A.New<UserDto>();
            var response = await browser.Post("/user", w => w.JsonBody(user));
            var result = response.Body.DeserializeJson<dynamic>();
            Assert.True(((string)result.Token).Length > 0);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            var newresponse = await browser.Post($"/user/confirm/{(string)result.Id}/{(string)result.Token}");
            Assert.True(newresponse.StatusCode == HttpStatusCode.OK);

        }

        [Fact]
        public async void Should_Generate_Token()
        {
           
            var user = A.New<UserDto>();
            var response = await browser.Post("/user", w => w.JsonBody(user));
            var result = response.Body.DeserializeJson<dynamic>();
            Assert.True(((string)result.Token).Length > 0);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            response = await browser.Get($"/user/{(string)result.Id}/token");
            var confirmResponse = response.Body.AsString();
            Assert.NotNull(confirmResponse);
            

        }
    }
}
