using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Server.API;
using MindNote.Server.API.Controllers;
using MindNote.Server.Share.Configuration;
using System;
using System.Linq;
using System.Text;

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
        [DataRow("/swagger/index.html")]
        [DataRow("/swagger/v1/swagger.json")]
        public void UrlsBasicTest(string url)
        {
            IDataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            var idData = new TestIdentityDataGetter("id", "email@host", "user");
            using (var testServer = new TestApiWebApplicationFactory<MindNote.Server.API.Startup>(provider, idData))
            {
                using (var client = testServer.CreateClient())
                {
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [DataTestMethod]
        [DataRow("/Nodes/All")]
        [DataRow("/Relations/All")]
        [DataRow("/Tags/All")]
        public void UrlsAuthTest(string url)
        {
            IDataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            var idData = new TestIdentityDataGetter("id", "email@host", "user");
            using (var testServer = new TestApiWebApplicationFactory<MindNote.Server.API.Startup>(provider, idData))
            {
                using (var client = testServer.CreateClient())
                {
                    var response = client.GetAsync(url).Result;
                    Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
                }
            }
        }

        [TestMethod]
        public void Nodes()
        {
            var provider = new MindNote.Data.Providers.InMemory.DataProvider();
            var idData = new TestIdentityDataGetter("id", "email@host", "user");
            var controller = new NodesController(provider, idData);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get(0).Result);
            controller.Clear().Wait();
            {
                var node = new Node { Name = "name" };
                int id = controller.Create(node).Result.Value;
                Assert.AreEqual(node.Name, controller.Query(id, null, null, null).Result.First().Name);
                node.Content = "content";
                Assert.IsTrue(controller.Update(id, node).Result.HasValue);
                Assert.IsTrue(controller.Delete(id).Result.HasValue);
            }
        }

        [TestMethod]
        public void Relations()
        {
            var provider = new MindNote.Data.Providers.InMemory.DataProvider();
            var idData = new TestIdentityDataGetter("id", "email@host", "user");
            int a = provider.NodesProvider.Create(new Node { Name = "node1" }, idData.GetClaimId(null)).Result.Value;
            int b = provider.NodesProvider.Create(new Node { Name = "node2" }, idData.GetClaimId(null)).Result.Value;
            var controller = new RelationsController(provider, idData);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get(0).Result);
            controller.Clear().Wait();
            {
                var rel = new Relation { From = a, To = b };
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
        public void Tags()
        {
            var provider = new MindNote.Data.Providers.InMemory.DataProvider();

            var idData = new TestIdentityDataGetter("id", "email@host", "user");
            var controller = new TagsController(provider, idData);
            Assert.IsFalse(controller.GetAll().Result.Any());
            Assert.IsNull(controller.Get(0).Result);
            controller.Clear().Wait();
            {
                var tag = new Tag { Name = "tag", Color = "black" };
                int id = controller.Create(tag).Result.Value;
                Assert.AreEqual(tag.Name, controller.Query(id, null, null).Result.First().Name);
                Assert.AreEqual(tag.Color, controller.GetByName(tag.Name).Result.Color);
                tag.Color = "white";
                Assert.IsTrue(controller.Update(id, tag).Result.HasValue);
                Assert.IsTrue(controller.Delete(id).Result.HasValue);
            }
        }
    }
}
