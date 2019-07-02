using IdentityServer4.Test;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Server.Identity;
using System;

namespace Test.Server.Identities
{
    [TestClass]
    public class Identity
    {
        [DataTestMethod]
        [DataRow("/Index")]
        [DataRow("/Privacy")]
        [DataRow("/Error")]
        [DataRow("/Identity/Account/Register")]
        [DataRow("/Identity/Account/Login")]
        [DataRow("/.well-known/openid-configuration")]
        public void Urls(string url)
        {
            using (TestServer testServer = new TestServer(Program.CreateWebHostBuilder(Array.Empty<string>())))
            {
                using (System.Net.Http.HttpClient client = testServer.CreateClient())
                {
                    System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [TestMethod]
        public void Token()
        {
            TestUser user = Utils.DefaultUser;
            using (MockIdentityWebApplicationFactory id = new MockIdentityWebApplicationFactory(user))
            {
                string token = id.GetBearerToken(user.Username, user.Password, Config.APIScope);
            }
        }
    }
}
