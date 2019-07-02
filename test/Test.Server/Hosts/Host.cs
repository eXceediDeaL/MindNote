using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers;
using MindNote.Server.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Test.Server.Apis;
using Test.Server.Identities;

namespace Test.Server.Hosts
{
    [TestClass]
    public class Host
    {
        public static IEnumerable<object[]> AuthGetUrls
        {
            get
            {
                List<string> res = new List<string>
                {
                    "/Identity/Login",
                };

                {
                    string[] sub = new string[]
                    {
                        "Index"
                    };
                    res.AddRange(sub.Select(x => "/Nodes/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                        "Index"
                    };
                    res.AddRange(sub.Select(x => "/Relations/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                        "Index"
                    };
                    res.AddRange(sub.Select(x => "/Tags/" + x));
                }
                return res.Select(x => new object[] { x });
            }
        }

        [DataTestMethod]
        [DataRow("/Index")]
        [DataRow("/Error")]
        public void Urls(string url)
        {
            using (TestServer testServer = new TestServer(MindNote.Server.Host.Program.CreateWebHostBuilder(Array.Empty<string>())))
            {
                using (HttpClient client = testServer.CreateClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(AuthGetUrls))]
        public void AuthGet(string url)
        {
            IDataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            IdentityServer4.Test.TestUser user = Utils.DefaultUser;
            using (MockIdentityWebApplicationFactory id = new MockIdentityWebApplicationFactory(user))
            {
                string token = id.GetBearerToken(user.Username, user.Password, Config.APIScope);
                MockTokenIdentityDataGetter idData = new MockTokenIdentityDataGetter(token);
                using (MockApiWebApplicationFactory api = new MockApiWebApplicationFactory(id.Server, provider, new IdentityDataGetter()))
                {
                    using (MockHostWebApplicationFactory<MindNote.Server.API.Startup> testServer = new MockHostWebApplicationFactory<MindNote.Server.API.Startup>(id.Server, api, idData))
                    {
                        using (HttpClient client = testServer.CreateClient())
                        {
                            HttpResponseMessage response = client.GetAsync(url).Result;
                            Assert.IsFalse(response.IsSuccessStatusCode);

                            client.SetBearerToken(token);
                            response = client.GetAsync(url).Result;
                            response.EnsureSuccessStatusCode();
                        }
                    }
                }
            }
        }
    }
}
