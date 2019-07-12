using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data.Mutations;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Data
{
    public class Tester
    {
        private readonly Random random;
        private const int TestCount = 10;
        private const int UserCount = 5;
        private const double DeleteRate = 0.5;

        public IDataRepository Repository
        {
            get; private set;
        }

        private readonly string[] usernames;

        public Tester(IDataRepository provider)
        {
            Repository = provider;
            random = new Random();
            usernames = Enumerable.Range(0, UserCount).Select(x => x.ToString()).ToArray();
        }

        public void Valid(RawNote item, string userId)
        {
            if (item.CategoryId.HasValue)
            {
                Assert.IsNotNull(Repository.Categories.Get(item.CategoryId.Value, userId).Result, "no tag with from id {0}", item.CategoryId.Value);
            }
        }

        public void Valid(RawCategory item)
        {
            Assert.IsFalse(string.IsNullOrEmpty(item.Name), "name is null/empty");
        }

        public void Clear()
        {
            foreach (var username in usernames)
            {
                Repository.Notes.Clear(username);
                Assert.IsFalse(Repository.Notes.Query(username).Result.Any(), "clear failed");
                Repository.Categories.Clear(username);
                Assert.IsFalse(Repository.Categories.Query(username).Result.Any(), "clear failed");
            }
        }

        private void NoteBasic()
        {
            INoteRepository pro = Repository.Notes;
            Clear();
            for (int i = 0; i < TestCount; i++)
            {
                var data = new RawNote
                {
                    Title = $"item {i}",
                    Content = $"item content {i}",
                    CategoryId = null,
                };
                string username = random.Choice(usernames);
                data.UserId = username;

                int id;
                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;

                    Assert.IsNotNull(pro.Get(id, username), "get after create failed");

                    var actual = pro.Get(id, username).Result;
                    Valid(actual, username);

                    data.Id = id;
                    Assert.AreEqual(data.Title, actual.Title, "get failed");
                    Assert.AreEqual(data.Content, actual.Content, "get failed");
                }

                {
                    data.Title = $"item {i} update";
                    data.Content = $"item {i} update";
                    Assert.AreEqual(id, pro.Update(id, new MindNote.Data.Mutations.MutationNote
                    {
                        Title = new Mutation<string>(data.Title),
                        Content = new Mutation<string>(data.Content)
                    }, username).Result, "update failed");

                    IEnumerable<RawNote> items = pro.Query(username, x => x.Id == id && x.Title == data.Title && x.Content == data.Content).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    RawNote actual = items.First();
                    Valid(actual, username);

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
                IEnumerable<RawNote> items = pro.Query(user).Result;
                foreach (RawNote v in items)
                {
                    Valid(v, user);
                }
            }
        }

        private void NoteFailed()
        {
            string username = random.Choice(usernames);
            Repository.Categories.Clear(username).Wait();
            var pro = Repository.Notes;
            Assert.IsFalse(pro.Create(new RawNote { Title = null }, username).Result >= 0, "node name is null");
            Assert.IsFalse(pro.Create(new RawNote { Title = "" }, username).Result >= 0, "node name is empty");
            Assert.IsFalse(pro.Create(new RawNote { Title = "a", CategoryId = 0 }, username).Result >= 0, "no this tag but create node");
        }

        private void CategoryBasic()
        {
            Clear();

            var pro = Repository.Categories;

            for (int i = 0; i < TestCount; i++)
            {
                var data = new RawCategory
                {
                    Name = $"item {i}",
                    Color = random.NextString(),
                };
                string username = random.Choice(usernames);
                data.UserId = username;

                int id;
                {
                    int? tid = pro.Create(data, username).Result;
                    Assert.IsTrue(tid.HasValue, "create failed");
                    id = tid.Value;
                    data.Id = id;

                    Assert.IsNotNull(pro.Get(id, ""), "get after create failed");

                    var actual = pro.Get(id, username).Result;
                    Valid(actual);

                    Assert.AreEqual(data.Id, actual.Id, "get failed");
                    Assert.AreEqual(data.Name, actual.Name, "get failed");
                    Assert.AreEqual(data.Color, actual.Color, "get failed");
                }

                {
                    data.Name = $"item {i} update";
                    data.Color = random.NextString();
                    Assert.AreEqual(id, pro.Update(id, new MutationCategory
                    {
                        Name = new Mutation<string>(data.Name),
                        Color = new Mutation<string>(data.Color)
                    }, username).Result, "update failed");

                    IEnumerable<RawCategory> items = pro.Query(username, x => x.Id == id && x.Name == data.Name && x.Color == data.Color).Result;
                    Assert.AreEqual(1, items.Count(), "query failed");
                    var actual = items.First();
                    Valid(actual);

                    Assert.AreEqual(data.Id, actual.Id, "query failed");
                    Assert.AreEqual(data.Name, actual.Name, "query failed");
                    Assert.AreEqual(data.Color, actual.Color, "query failed");
                }

                if (random.NextDouble() < DeleteRate)
                {
                    Assert.IsNotNull(pro.Delete(id, username).Result, "delete failed");
                    Assert.IsNull(pro.Get(id, username).Result, "get after delete failed");
                }
            }

            foreach (string user in usernames)
            {
                var items = pro.Query(user).Result;
                foreach (var v in items)
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

        public void CategoryIndependent()
        {
            CategoryBasic();
            CategoryFailed();
        }

        public void UserIndependent()
        {
            var provider = Repository.Users;
            provider.Clear(null).Wait();
            Assert.IsFalse(provider.Query(null).Result.Any());
            foreach (var v in usernames)
            {
                Assert.AreEqual(v, provider.Create(new RawUser { Id = v }, v).Result);
                Assert.IsNotNull(provider.Get(v, v).Result);
                Assert.AreEqual(v, provider.Update(v, new MutationUser(), v).Result);
                Assert.AreEqual(v, provider.Delete(v, v).Result);
                Assert.IsNull(provider.Get(v, v).Result);
            }
        }
    }
}
