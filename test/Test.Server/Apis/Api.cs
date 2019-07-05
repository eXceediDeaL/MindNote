using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Client.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Server.API.Controllers;
using MindNote.Server.Identity;
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
        public static IEnumerable<object[]> AuthGetUrls
        {
            get
            {
                List<string> res = new List<string>();
                {
                    string[] sub = new string[]
                    {
                    "All",
                    "Query",
                    "0",
                    "1"
                    };
                    res.AddRange(sub.Select(x => "/Notes/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                    "All",
                    "Query",
                    "0",
                    "1",
                    "Adjacents/0",
                    "Adjacents/1",
                    };
                    res.AddRange(sub.Select(x => "/Relations/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                    "All",
                    "Query",
                    "0",
                    "1"
                    };
                    res.AddRange(sub.Select(x => "/Categories/" + x));
                }
                {
                    string[] sub = new string[]
                    {
                    "All",
                    "user"
                    };
                    res.AddRange(sub.Select(x => "/Users/" + x));
                }
                return res.Select(x => new object[] { x });
            }
        }

        [DataTestMethod]
        [DataRow("/swagger/index.html")]
        [DataRow("/swagger/v1/swagger.json")]
        [DynamicData(nameof(AuthGetUrls))]
        public void Urls(string url)
        {
            Utils.UseApiEnvironment((_, api, __) =>
            {
                using (HttpClient client = api.CreateClient())
                {
                    // TODO Relations not support unauth access
                    if (url.Contains("Relations")) return;
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            });
        }

        [DataTestMethod]
        [DynamicData(nameof(AuthGetUrls))]
        public void AuthGet(string url)
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (HttpClient client = api.CreateClient())
                {
                    // HttpResponseMessage response = client.GetAsync(url).Result;
                    // Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

                    client.SetBearerToken(token);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }, Utils.SampleOneUserDataProvider(Utils.DefaultUser.SubjectId));
        }

        [TestMethod]
        public void Notes()
        {
            MindNote.Data.Providers.InMemory.DataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            NotesController controller = new NotesController(provider, Utils.MockIdentityDataGetter);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get(0).Result);
            controller.Clear().Wait();
            {
                Note node = new Note { Title = "name" };
                int id = controller.Create(node).Result.Value;
                Assert.AreEqual(node.Title, controller.Query(id, null, null, null, null, null, null, null, null).Result.First().Title);
                node.Content = "content";
                Assert.IsTrue(controller.Update(id, node).Result.HasValue);
                Assert.IsTrue(controller.Delete(id).Result.HasValue);
            }
        }

        [TestMethod]
        public void Relations()
        {
            MindNote.Data.Providers.InMemory.DataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            int a = provider.NotesProvider.Create(new Note { Title = "node1" }, Utils.MockIdentityDataGetter.GetClaimId(null)).Result.Value;
            int b = provider.NotesProvider.Create(new Note { Title = "node2" }, Utils.MockIdentityDataGetter.GetClaimId(null)).Result.Value;
            RelationsController controller = new RelationsController(provider, Utils.MockIdentityDataGetter);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get(0).Result);
            controller.Clear().Wait();
            {
                Relation rel = new Relation { From = a, To = b };
                int id = controller.Create(rel).Result.Value;
                Assert.AreEqual(rel.From, controller.Query(id, null, null).Result.First().From);
                Assert.AreEqual(1, controller.GetAdjacents(b).Result.Count());
                rel.To = a;
                Assert.IsTrue(controller.Update(id, rel).Result.HasValue);
                Assert.AreEqual(0, controller.GetAdjacents(b).Result.Count());
                Assert.IsTrue(controller.Delete(id).Result.HasValue);
                Assert.IsTrue(controller.ClearAdjacents(a).Result.HasValue);
            }
        }

        [TestMethod]
        public void Categories()
        {
            MindNote.Data.Providers.InMemory.DataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();

            CategoriesController controller = new CategoriesController(provider, Utils.MockIdentityDataGetter);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get(0).Result);
            controller.Clear().Wait();
            {
                Category tag = new Category { Name = "tag", Color = "black" };
                int id = controller.Create(tag).Result.Value;
                Assert.AreEqual(tag.Name, controller.Query(id, null, null, null).Result.First().Name);
                Assert.AreEqual(tag.Color, controller.Get(id).Result.Color);
                tag.Color = "white";
                Assert.IsTrue(controller.Update(id, tag).Result.HasValue);
                Assert.IsTrue(controller.Delete(id).Result.HasValue);
            }
        }

        [TestMethod]
        public void Users()
        {
            MindNote.Data.Providers.InMemory.DataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();

            UsersController controller = new UsersController(provider, Utils.MockIdentityDataGetter);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get("0").Result);
            controller.Clear().Wait();
            {
                User tag = new User { Name = "user" };
                string id = controller.Create(Guid.NewGuid().ToString(), tag).Result;
                Assert.AreEqual(tag.Name, controller.Get(id).Result?.Name);
                tag.Name = "user2";
                Assert.IsNotNull(controller.Update(id, tag).Result);
                Assert.IsNotNull(controller.Delete(id).Result);
            }
        }
    }
}
