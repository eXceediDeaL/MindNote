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

        public void Valid(Note item, string userId)
        {
            if (item.CategoryId.HasValue)
            {
                Assert.IsNotNull(Provider.CategoriesProvider.Get(item.CategoryId.Value, userId).Result, "no tag with from id {0}", item.CategoryId.Value);
            }
        }

        public void Valid(Relation item, string userId)
        {
            Assert.IsNotNull(Provider.NotesProvider.Get(item.From, userId).Result, "no node with from id {0}", item.From);
            Assert.IsNotNull(Provider.NotesProvider.Get(item.To, userId).Result, "no node with to id {0}", item.To);
        }

        public void Valid(Category item)
        {
            Assert.IsFalse(string.IsNullOrEmpty(item.Name), "name is null/empty");
        }

        public void Clear()
        {
            foreach (var username in usernames)
            {
                Provider.NotesProvider.Clear(username);
                Assert.IsFalse(Provider.NotesProvider.GetAll(username).Result.Any(), "clear failed");
                Provider.RelationsProvider.Clear(username);
                Assert.IsFalse(Provider.RelationsProvider.GetAll(username).Result.Any(), "clear failed");
                Provider.CategoriesProvider.Clear(username);
                Assert.IsFalse(Provider.CategoriesProvider.GetAll(username).Result.Any(), "clear failed");
            }
        }

        private void NoteBasic()
        {
            INotesProvider pro = Provider.NotesProvider;
            Clear();
            for (int i = 0; i < TestCount; i++)
            {
                Note data = new Note
                {
                    Title = $"item {i}",
                    Content = $"item content {i}",
                    CategoryId = null,
                };
                string username = random.Choice(usernames);

                Assert.AreEqual(data, data.Clone(), "clone failed");

                int id;
                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, username), "get after create failed");

                    Note actual = pro.Get(id, username).Result;
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data.Title, actual.Title, "get failed");
                    Assert.AreEqual(data.Content, actual.Content, "get failed");
                }

                {
                    data.Title = $"item {i} update";
                    data.Content = $"item {i} update";
                    data.Id = random.Next();
                    Assert.AreEqual(id, pro.Update(id, data, username).Result, "update failed");

                    IEnumerable<Note> items = pro.Query(id, data.Title, data.Content, null, null, null, null, null, username).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    Note actual = items.First();
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data.Title, actual.Title, "query failed");
                    Assert.AreEqual(data.Content, actual.Content, "query failed");
                }

                if (random.NextDouble() < DeleteRate)
                {
                    Assert.IsNotNull(pro.Delete(id, username).Result, "delete failed");
                    Assert.IsNull(pro.Get(id, username).Result, "get after delete failed");
                }
            }

            foreach (string user in usernames)
            {
                IEnumerable<Note> items = pro.GetAll(user).Result;
                foreach (Note v in items)
                {
                    Valid(v, user);
                }
            }
        }

        private void NoteFailed()
        {
            string username = random.Choice(usernames);
            Provider.CategoriesProvider.Clear(username).Wait();
            INotesProvider pro = Provider.NotesProvider;
            Assert.IsNull(pro.Create(new Note { Title = null }, username).Result, "node name is null");
            Assert.IsNull(pro.Create(new Note { Title = "" }, username).Result, "node name is empty");
            Assert.IsNull(pro.Create(new Note { Title = "a", CategoryId = 0 }, username).Result, "no this tag but create node");
        }

        private void RelationBasic()
        {
            Clear();

            INotesProvider npro = Provider.NotesProvider;

            IRelationsProvider pro = Provider.RelationsProvider;

            string[] usernames = new string[] { "user" };

            List<int> nodes = new List<int>();

            for (int i = 0; i < TestCount; i++)
            {
                Note data = new Note
                {
                    Title = $"item {i}",
                    Content = $"item content {i}",
                    CategoryId = null,
                };
                string username = random.Choice(usernames);
                int? tid = npro.Create(data, username).Result;
                Assert.IsTrue(tid.HasValue, "create failed");
                nodes.Add(tid.Value);
            }

            for (int i = 0; i < TestCount; i++)
            {
                Relation data = new Relation
                {
                    From = random.Choice(nodes),
                    To = random.Choice(nodes),
                };
                string username = random.Choice(usernames);
                Assert.AreEqual(data, data.Clone());

                int id;
                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, username), "get after create failed");

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
                    Assert.IsNull(pro.Get(id, username).Result, "get after delete failed");
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
            string username = random.Choice(usernames);
            Provider.CategoriesProvider.Clear(username).Wait();
            int id = Provider.NotesProvider.Create(new Note { Title = "name" }, username).Result.Value;
            IRelationsProvider pro = Provider.RelationsProvider;
            Assert.IsNull(pro.Create(new Relation { From = id - 1, To = id + 1 }, username).Result);
            Assert.IsNull(pro.Create(new Relation { From = id, To = id + 1 }, username).Result);
            Assert.IsNull(pro.Create(new Relation { From = id - 1, To = id }, username).Result);
        }

        private void RelationSpecial()
        {
            Clear();

            INotesProvider npro = Provider.NotesProvider;

            IRelationsProvider pro = Provider.RelationsProvider;

            string[] usernames = new string[] { "user" };

            List<(int, string)> nodes = new List<(int, string)>();
            Dictionary<int, HashSet<int>> adjs = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < TestCount; i++)
            {
                Note data = new Note
                {
                    Title = $"item {i}",
                    Content = $"item content {i}",
                    CategoryId = null,
                };
                string username = random.Choice(usernames);
                int? tid = npro.Create(data, username).Result;
                Assert.IsTrue(tid.HasValue, "create failed");
                nodes.Add((tid.Value, username));
                adjs.Add(tid.Value, new HashSet<int>());
            }

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

        private void CategoryBasic()
        {
            Clear();

            ICategoriesProvider pro = Provider.CategoriesProvider;

            for (int i = 0; i < TestCount; i++)
            {
                Category data = new Category
                {
                    Name = $"item {i}",
                    Color = random.NextString(),
                };
                string username = random.Choice(usernames);
                Assert.AreEqual(data, data.Clone());

                int id;
                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, ""), "get after create failed");

                    Category actual = pro.Get(id, username).Result;
                    Valid(actual);
                    {
                        Category tname = pro.GetByName(data.Name, username).Result;
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

                    IEnumerable<Category> items = pro.Query(id, data.Name, data.Color, username).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    Category actual = items.First();
                    Valid(actual);

                    data.Id = id;
                    Assert.AreEqual(data, actual, "query failed");
                }

                if (random.NextDouble() < DeleteRate)
                {
                    Assert.IsNotNull(pro.Delete(id, username).Result, "delete failed");
                    Assert.IsNull(pro.Get(id, username).Result, "get after delete failed");
                }
            }

            foreach (string user in usernames)
            {
                IEnumerable<Category> items = pro.GetAll(user).Result;
                foreach (Category v in items)
                {
                    Valid(v);
                }
            }
        }

        private void CategoryFailed()
        {
        }

        public void NoteIndependent()
        {
            NoteBasic();
            NoteFailed();
        }

        public void RelationIndependent()
        {
            RelationBasic();
            RelationSpecial();
            RelationFailed();
        }

        public void CategoryIndependent()
        {
            CategoryBasic();
            CategoryFailed();
        }
    }
}
