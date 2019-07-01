using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Server.Identity;
using System;

namespace Test.Server
{
    [TestClass]
    public class Identity
    {
        [DataTestMethod]
        [DataRow("/Index")]
        [DataRow("/Privacy")]
        [DataRow("/Identity/Account/Register")]
        [DataRow("/Identity/Account/Login")]
        [DataRow("/.well-known/openid-configuration")]
        public void Urls(string url)
        {
            using (TestServer testServer = new TestServer(Program.CreateWebHostBuilder(Array.Empty<string>())))
            {
                using(var client = testServer.CreateClient())
                {
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
