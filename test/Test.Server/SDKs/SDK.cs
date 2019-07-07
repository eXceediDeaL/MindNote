using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Server.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Test.Server.Apis;
using Test.Server.Identities;

namespace Test.Server.SDKs
{
    [TestClass]
    public class SDK
    {
        [TestMethod]
        public void Notes()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    INotesClient client = new NotesClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, 0).Result);
                    client.Clear(token).Wait();
                    {
                        Note node = new Note { Title = "name" };
                        int id = client.Create(token, node).Result.Value;
                        Assert.AreEqual(node.Title, client.Query(token, id).Result.First().Title);
                        node.Content = "content";
                        Assert.IsTrue(client.Update(token, id, node).Result.HasValue);
                        Assert.IsTrue(client.Delete(token, id).Result.HasValue);
                    }
                }
            });
        }

        [TestMethod]
        public void Categories()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    ICategoriesClient client = new CategoriesClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, 0).Result);
                    client.Clear(token).Wait();
                    {
                        Category tag = new Category { Name = "tag", Color = "black" };
                        int id = client.Create(token, tag).Result.Value;
                        Assert.AreEqual(tag.Name, client.Query(token, id).Result.First().Name);
                        Assert.AreEqual(tag.Color, client.Get(token, id).Result.Color);
                        tag.Color = "white";
                        Assert.IsTrue(client.Update(token, id, tag).Result.HasValue);
                        Assert.IsTrue(client.Delete(token, id).Result.HasValue);
                    }
                }
            });
        }

        [TestMethod]
        public void Relations()
        {
            MindNote.Data.Providers.InMemory.DataProvider provider = new MindNote.Data.Providers.InMemory.DataProvider();
            int a = provider.NotesProvider.Create(new MindNote.Data.Note { Title = "node1" }, Utils.DefaultUser.SubjectId).Result.Value;
            int b = provider.NotesProvider.Create(new MindNote.Data.Note { Title = "node2" }, Utils.DefaultUser.SubjectId).Result.Value;
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    IRelationsClient client = new RelationsClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, 0).Result);
                    client.Clear(token).Wait();
                    {
                        Relation rel = new Relation { From = a, To = b };
                        int id = client.Create(token, rel).Result.Value;
                        Assert.AreEqual(rel.From, client.Query(token, id, null, null).Result.First().From);
                        Assert.AreEqual(1, client.GetAdjacents(token, b).Result.Count());
                        rel.To = a;
                        Assert.IsTrue(client.Update(token, id, rel).Result.HasValue);
                        Assert.AreEqual(0, client.GetAdjacents(token, b).Result.Count());
                        Assert.IsTrue(client.Delete(token, id).Result.HasValue);
                        Assert.IsTrue(client.ClearAdjacents(token, a).Result.HasValue);
                    }
                }
            }, provider);
        }

        [TestMethod]
        public void Users()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    IUsersClient client = new UsersClient(baseClient);

                    Assert.IsFalse(client.GetAll(token).Result.Any());
                    Assert.IsNull(client.Get(token, "0").Result);
                    client.Clear(token).Wait();
                    {
                        User tag = new User { Name = "user" };
                        string id = client.Create(token, Guid.NewGuid().ToString(), tag).Result;
                        Assert.AreEqual(tag.Name, client.Get(token, id).Result?.Name);
                        tag.Name = "user2";
                        Assert.IsNotNull(client.Update(token, id, tag).Result);
                        Assert.IsNotNull(client.Delete(token, id).Result);
                    }
                }
            });
        }
    }
}
