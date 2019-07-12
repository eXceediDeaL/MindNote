using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Backend.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Test.Server.Apis;
using Test.Server.Identities;
using MindNote.Data.Raws;

namespace Test.Server.SDKs
{
    [TestClass]
    public class SDK
    {
        [TestMethod]
        public void RawGraphQLClient()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    var client = new GraphQLClient(baseClient, new MockGraphQLClientOptions(new Uri(baseClient.BaseAddress.ToString() + "graphql"), token));
                    Assert.ThrowsException<AggregateException>(() =>
                    {
                        client.Query(new GraphQL.Common.Request.GraphQLRequest
                        {
                            Query = "mutation"
                        }).Wait();
                    });
                    Assert.ThrowsException<AggregateException>(() =>
                    {
                        client.Mutation(new GraphQL.Common.Request.GraphQLRequest
                        {
                            Query = "query"
                        }).Wait();
                    });
                }
            });
        }

        [TestMethod]
        public void Notes()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    var graphqlClient = new GraphQLClient(baseClient, new MockGraphQLClientOptions(new Uri(baseClient.BaseAddress.ToString() + "graphql"), token));
                    INotesClient client = new NotesClient(graphqlClient);

                    Assert.IsFalse(client.Query().Result.Nodes.Any());
                    Assert.IsNull(client.Get(0).Result);
                    client.Clear().Wait();
                    {
                        var node = new RawNote { Title = "name" };
                        int id = client.Create(node).Result.Value;
                        Assert.AreEqual(node.Title, client.Query(id).Result.Nodes.First().Title);
                        Assert.IsFalse(client.Query(id, "", "", 0, "").Result.Nodes.Any());
                        node.Content = "content";
                        Assert.IsTrue(client.Update(id, new MindNote.Data.Mutations.MutationNote
                        {
                        }).Result.HasValue);
                        Assert.IsTrue(client.Delete(id).Result.HasValue);
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
                    var graphqlClient = new GraphQLClient(baseClient, new MockGraphQLClientOptions(new Uri(baseClient.BaseAddress.ToString() + "graphql"), token));
                    ICategoriesClient client = new CategoriesClient(graphqlClient);

                    Assert.IsFalse(client.Query().Result.Nodes.Any());
                    Assert.IsNull(client.Get(0).Result);
                    client.Clear().Wait();
                    {
                        var tag = new RawCategory { Name = "tag", Color = "black" };
                        int id = client.Create(tag).Result.Value;
                        Assert.AreEqual(tag.Name, client.Query(id).Result.Nodes.First().Name);
                        Assert.IsFalse(client.Query(id, "", "", "").Result.Nodes.Any());
                        Assert.AreEqual(tag.Color, client.Get(id).Result.Color);
                        tag.Color = "white";
                        Assert.IsTrue(client.Update(id, new MindNote.Data.Mutations.MutationCategory
                        {
                        }).Result.HasValue);
                        Assert.IsTrue(client.Delete(id).Result.HasValue);
                    }
                }
            });
        }


        [TestMethod]
        public void Users()
        {
            Utils.UseApiEnvironment((_, api, token) =>
            {
                using (var baseClient = api.CreateClient())
                {
                    var graphqlClient = new GraphQLClient(baseClient, new MockGraphQLClientOptions(new Uri(baseClient.BaseAddress.ToString() + "graphql"), token));
                    IUsersClient client = new UsersClient(graphqlClient);

                    // Assert.IsFalse(client.Query().Result.Any());
                    Assert.IsNull(client.Get("0").Result);
                    client.Clear().Wait();
                    {
                        var tag = new RawUser { Name = "user", Id = Guid.NewGuid().ToString() };
                        string id = client.Create(tag).Result;
                        Assert.AreEqual(tag.Name, client.Get(id).Result?.Name);
                        tag.Name = "user2";
                        Assert.IsNotNull(client.Update(id, new MindNote.Data.Mutations.MutationUser
                        {
                        }).Result);
                        Assert.IsNotNull(client.Delete(id).Result);
                    }
                }
            });
        }
    }
}
