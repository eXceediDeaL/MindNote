using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data;
using MindNote.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.Data
{
    public class Tester
    {
        private readonly Random random;
        private const int TestCount = 10;
        private const int UserCount = 5;
        private const double DeleteRate = 0.5;

        public IDataProvider Provider
        {
            get; private set;
        }

        private readonly string[] usernames;

        public Tester(IDataProvider provider)
        {
            Provider = provider;
            random = new Random();
            usernames = Enumerable.Range(0, UserCount).Select(x => x.ToString()).ToArray();
        }

        public void Valid(Node item, string userId)
        {
            if (item.TagId.HasValue)
            {
                Assert.IsNotNull(Provider.TagsProvider.Get(item.TagId.Value, userId).Result, "no tag with from id {0}", item.TagId.Value);
            }
        }

        public void Valid(Relation item, string userId)
        {
            Assert.IsNotNull(Provider.NodesProvider.Get(item.From, userId).Result, "no node with from id {0}", item.From);
            Assert.IsNotNull(Provider.NodesProvider.Get(item.To, userId).Result, "no node with to id {0}", item.To);
        }

        public void Valid(Tag item)
        {
            Assert.IsFalse(string.IsNullOrEmpty(item.Name), "name is null/empty");
        }

        private void NodeBasic()
        {
            INodesProvider pro = Provider.NodesProvider;

            pro.Clear();
            Assert.IsFalse(pro.GetAll().Result.Any(), "clear failed");

            for (int i = 0; i < TestCount; i++)
            {
                Node data = new Node
                {
                    Name = $"item {i}",
                    Content = $"item content {i}",
                    TagId = null,
                };
                string username = random.Choice(usernames);
                int id = 0;

                Assert.AreEqual(data, data.Clone(), "clone failed");

                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, ""), "get after create failed");

                    Node actual = pro.Get(id, username).Result;
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data, actual, "get failed");
                    Assert.AreEqual(data.GetHashCode(), actual.GetHashCode(), "get failed");
                }

                {
                    data.Name = $"item {i} update";
                    data.Content = $"item {i} update";
                    data.Id = random.Next();
                    Assert.AreEqual(id, pro.Update(id, data, username).Result, "update failed");

                    IEnumerable<Node> items = pro.Query(id, data.Name, data.Content, null, username).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    Node actual = items.First();
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data, actual, "query failed");
                }

                if (random.NextDouble() < DeleteRate)
                {
                    Assert.IsNotNull(pro.Delete(id, username).Result, "delete failed");
                    Assert.IsNull(pro.Get(id).Result, "get after delete failed");
                }
            }

            foreach (string user in usernames)
            {
                IEnumerable<Node> items = pro.GetAll(user).Result;
                foreach (Node v in items)
                {
                    Valid(v, user);
                }
            }
        }

        private void NodeFailed()
        {
            Provider.TagsProvider.Clear().Wait();
            INodesProvider pro = Provider.NodesProvider;
            Assert.IsNull(pro.Create(new Node { Name = null }).Result, "node name is null");
            Assert.IsNull(pro.Create(new Node { Name = "" }).Result, "node name is empty");
            Assert.IsNull(pro.Create(new Node { Name = "a", TagId = 0 }).Result, "no this tag but create node");
        }

        private void RelationBasic()
        {
            INodesProvider npro = Provider.NodesProvider;

            npro.Clear();
            Assert.IsFalse(npro.GetAll().Result.Any(), "clear failed");

            IRelationsProvider pro = Provider.RelationsProvider;

            string[] usernames = new string[] { "user" };

            List<int> nodes = new List<int>();

            for (int i = 0; i < TestCount; i++)
            {
                Node data = new Node
                {
                    Name = $"item {i}",
                    Content = $"item content {i}",
                    TagId = null,
                };
                string username = random.Choice(usernames);
                int? tid = npro.Create(data, username).Result;
                Assert.IsTrue(tid.HasValue, "create failed");
                nodes.Add(tid.Value);
            }

            pro.Clear();
            Assert.IsFalse(pro.GetAll().Result.Any(), "clear failed");

            for (int i = 0; i < TestCount; i++)
            {
                Relation data = new Relation
                {
                    From = random.Choice(nodes),
                    To = random.Choice(nodes),
                };
                string username = random.Choice(usernames);
                int id = 0;

                Assert.AreEqual(data, data.Clone());

                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, ""), "get after create failed");

                    Relation actual = pro.Get(id, username).Result;
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data, actual, "get failed");
                    Assert.AreEqual(data.GetHashCode(), actual.GetHashCode(), "get failed");
                }

                {
                    data.From = random.Choice(nodes);
                    data.To = random.Choice(nodes);
                    data.Id = random.Next();
                    Assert.AreEqual(id, pro.Update(id, data, username).Result, "update failed");

                    IEnumerable<Relation> items = pro.Query(id, data.From, data.To, username).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    Relation actual = items.First();
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data, actual, "query failed");
                }

                if (random.NextDouble() < DeleteRate)
                {
                    Assert.IsNotNull(pro.Delete(id, username).Result, "delete failed");
                    Assert.IsNull(pro.Get(id).Result, "get after delete failed");
                }
            }

            foreach (string user in usernames)
            {
                IEnumerable<Relation> items = pro.GetAll(user).Result;
                foreach (Relation v in items)
                {
                    Valid(v, user);
                }
            }
        }

        private void RelationFailed()
        {
            Provider.NodesProvider.Clear().Wait();
            int id = Provider.NodesProvider.Create(new Node { Name = "name" }).Result.Value;
            IRelationsProvider pro = Provider.RelationsProvider;
            Assert.IsNull(pro.Create(new Relation { From = id - 1, To = id + 1 }).Result);
            Assert.IsNull(pro.Create(new Relation { From = id, To = id + 1 }).Result);
            Assert.IsNull(pro.Create(new Relation { From = id - 1, To = id }).Result);
        }

        private void RelationSpecial()
        {
            INodesProvider npro = Provider.NodesProvider;

            npro.Clear();
            Assert.IsFalse(npro.GetAll().Result.Any(), "clear failed");

            IRelationsProvider pro = Provider.RelationsProvider;

            string[] usernames = new string[] { "user" };

            List<(int, string)> nodes = new List<(int, string)>();
            Dictionary<int, HashSet<int>> adjs = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < TestCount; i++)
            {
                Node data = new Node
                {
                    Name = $"item {i}",
                    Content = $"item content {i}",
                    TagId = null,
                };
                string username = random.Choice(usernames);
                int? tid = npro.Create(data, username).Result;
                Assert.IsTrue(tid.HasValue, "create failed");
                nodes.Add((tid.Value, username));
                adjs.Add(tid.Value, new HashSet<int>());
            }

            pro.Clear();
            Assert.IsFalse(pro.GetAll().Result.Any(), "clear failed");

            for (int i = 0; i < TestCount; i++)
            {
                Relation data = new Relation
                {
                    From = random.Choice(nodes).Item1,
                    To = random.Choice(nodes).Item1,
                };
                string username = random.Choice(usernames);

                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    adjs[data.From].Add(data.To);
                    adjs[data.To].Add(data.From);
                }
            }

            foreach ((int id, string user) in nodes)
            {
                IEnumerable<Relation> actual = pro.GetAdjacents(id, user).Result;
                CollectionAssert.AreEquivalent(adjs[id].ToArray(), new HashSet<int>(actual.Select(x => x.From ^ x.To ^ id)).ToArray());
            }
            foreach ((int id, string user) in nodes)
            {
                pro.ClearAdjacents(id, user).Wait();
                Assert.IsFalse(pro.GetAdjacents(id, user).Result.Any());
            }
        }

        private void TagBasic()
        {
            ITagsProvider pro = Provider.TagsProvider;

            pro.Clear();
            Assert.IsFalse(pro.GetAll().Result.Any(), "clear failed");

            for (int i = 0; i < TestCount; i++)
            {
                Tag data = new Tag
                {
                    Name = $"item {i}",
                    Color = random.NextString(),
                };
                string username = random.Choice(usernames);
                int id = 0;

                Assert.AreEqual(data, data.Clone());

                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, ""), "get after create failed");

                    Tag actual = pro.Get(id, username).Result;
                    Valid(actual);
                    {
                        Tag tname = pro.GetByName(data.Name, username).Result;
                        Assert.AreEqual(actual, tname);
                    }

                    data.Id = id;
                    Assert.AreEqual(data, actual, "get failed");
                    Assert.AreEqual(data.GetHashCode(), actual.GetHashCode(), "get failed");
                }

                {
                    data.Name = $"item {i} update";
                    data.Color = random.NextString();
                    data.Id = random.Next();
                    Assert.AreEqual(id, pro.Update(id, data, username).Result, "update failed");

                    IEnumerable<Tag> items = pro.Query(id, data.Name, data.Color, username).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    Tag actual = items.First();
                    Valid(actual);

                    data.Id = id;
                    Assert.AreEqual(data, actual, "query failed");
                }

                if (random.NextDouble() < DeleteRate)
                {
                    Assert.IsNotNull(pro.Delete(id, username).Result, "delete failed");
                    Assert.IsNull(pro.Get(id).Result, "get after delete failed");
                }
            }

            foreach (string user in usernames)
            {
                IEnumerable<Tag> items = pro.GetAll(user).Result;
                foreach (Tag v in items)
                {
                    Valid(v);
                }
            }
        }

        private void TagFailed()
        {
        }

        public void NodeIndependent()
        {
            NodeBasic();
            NodeFailed();
        }

        public void RelationIndependent()
        {
            RelationBasic();
            RelationSpecial();
            RelationFailed();
        }

        public void TagIndependent()
        {
            TagBasic();
            TagFailed();
        }
    }
}
