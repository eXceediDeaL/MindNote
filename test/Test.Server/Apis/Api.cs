using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Backend.API.Controllers;
using MindNote.Backend.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Test.Server.Identities;

namespace Test.Server.Apis
{
    [TestClass]
    public class Api
    {
        [DataTestMethod]
        [DataRow("/swagger/index.html")]
        [DataRow("/swagger/v1/swagger.json")]
        public void Urls(string url)
        {
            Utils.UseApiEnvironment((_, api, __) =>
            {
                using (HttpClient client = api.CreateClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            });
        }
    }
}
