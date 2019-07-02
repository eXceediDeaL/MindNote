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
                List<string> res = new List<string>();

                {
                    string[] sub = new string[]
                    {
                        "Index",
                    };
                    res.AddRange(sub.Select(x => "/Identity/Account/" + x));
                }

                {
                    string[] sub = new string[]
                    {
                        "Index",
                        "Edit",
                    };
                    res.AddRange(sub.Select(x => "/Nodes/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                        "Index",
                        "Edit",
                    };
                    res.AddRange(sub.Select(x => "/Relations/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                        "Index",
                        "Edit",
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
            Utils.UseHostEnvironment((host, _) =>
            {
                using (HttpClient client = host.CreateClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            });
        }

        [DataTestMethod]
        [DynamicData(nameof(AuthGetUrls))]
        public void AuthGet(string url)
        {
            Utils.UseHostEnvironment((host, token) =>
            {
                using (HttpClient client = host.CreateClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    Assert.IsFalse(response.IsSuccessStatusCode);

                    client.SetBearerToken(token);
                    response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            });
        }
    }
}
