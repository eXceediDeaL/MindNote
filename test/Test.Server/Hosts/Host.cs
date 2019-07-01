using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers;
using MindNote.Server.Host;
using System;
using Test.Server.Apis;

namespace Test.Server.Hosts
{
    [TestClass]
    public class Host
    {
        [DataTestMethod]
        [DataRow("/Index")]
        public void Urls(string url)
        {
            using (TestServer testServer = new TestServer(Program.CreateWebHostBuilder(Array.Empty<string>())))
            {
                using (var client = testServer.CreateClient())
                {
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [DataTestMethod]
        [DataRow("/Index")]
        //[DataRow("/Nodes/Index")]
        //[DataRow("/Relations/Index")]
        //[DataRow("/Tags/Index")]
        public void Get(string url)
        {
            IDataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            var idData = new TestIdentityDataGetter("id", "email@host", "user");
            using (var api = new TestApiWebApplicationFactory<MindNote.Server.API.Startup>(provider, idData))
            {
                using (var testServer = new TestHostWebApplicationFactory<Startup, MindNote.Server.API.Startup>(api, idData))
                {
                    using (var client = testServer.CreateClient())
                    {
                        var response = client.GetAsync(url).Result;
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }
    }
}
