using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data;
using MindNote.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using MindNote.API.Controllers;

namespace Test.Unit
{
    [TestClass]
    public class SqlServerProvider
    {
        [TestMethod]
        public void Nodes()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db_node");
            var options = builder.Options;

            var tn = Helper.SampleNode();

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                var controller = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context).GetNodesProvider();

                Assert.AreEqual(0, controller.GetTags(0).Result.Count());

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

                tn.Id = id;

                context.Nodes.Find(id).Decode();

                controller.SetTags(id, new Tag[] { new Tag { Name = "app" } });

                Helper.NodeEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                Helper.NodeEqual(tn, controller.GetAll().Result.First(), true);
                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                Helper.NodeEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }

        [TestMethod]
        public void Structs()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db_struct");
            var options = builder.Options;

            var tn = Helper.SampleStruct();

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                var controller = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context).GetStructsProvider();
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

                tn.Id = id;

                context.Structs.Find(id).Decode();

                Helper.StructEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                Helper.StructEqual(tn, controller.GetAll().Result.First(), true);

                controller.GetFull(id).Wait();

                controller.SetTags(id, new Tag[] { new Tag { Name = "app" } });

                controller.SetRelations(id, new Relation[] { new Relation { From = 1, To = 1 } });

                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                Helper.StructEqual(tn, controller.Get(id).Result);

                controller.GetContent(id);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }

        [TestMethod]
        public void Tags()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db_tag");
            var options = builder.Options;

            var tn = Helper.SampleTag();

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                var controller = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context).GetTagsProvider();
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

                tn.Id = id;

                Helper.TagEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                Helper.TagEqual(tn, controller.GetAll().Result.First(), true);

                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                Helper.TagEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }

        [TestMethod]
        public void Relations()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db_relation");
            var options = builder.Options;

            var tn = Helper.SampleRelation();

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                var controller = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context).GetRelationsProvider();
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

                tn.Id = id;

                Helper.RelationEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                Helper.RelationEqual(tn, controller.GetAll().Result.First(), true);

                tn.Color = "whilte";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                Helper.RelationEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }
    }
}
