using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.API.Controllers;
using MindNote.Data;
using MindNote.Data.Providers;
using System;
using System.Linq;

namespace Test.Integration
{
    [TestClass]
    public class Controller
    {
        Node SampleNode()
        {
            int id = 100;
            string name = "testname", content = "testcontent";
            DateTimeOffset ct = DateTimeOffset.Now, mt = DateTimeOffset.Now;
            string[] tags = new string[] { name };
            return new Node
            {
                Id = id,
                Name = name,
                Content = content,
                CreationTime = ct,
                ModificationTime = mt,
                Tags = tags,
            };
        }

        Struct SampleStruct()
        {
            int id = 100;
            string name = "testname";
            DateTimeOffset ct = DateTimeOffset.Now, mt = DateTimeOffset.Now;
            string[] tags = new string[] { name };
            Relation[] rs = new Relation[]
            {
                new Relation(new int[]{1,2,3}),
                new Relation(new int[]{4,5,6})
            };
            return new Struct
            {
                Id = id,
                Name = name,
                Data = rs,
                CreationTime = ct,
                ModificationTime = mt,
                Tags = tags,
            };
        }

        void NodeEqual(Node expected, Node real)
        {
            Assert.AreEqual(expected.Id, expected.Id);
            Assert.AreEqual(expected.Name, expected.Name);
            Assert.AreEqual(expected.Content, expected.Content);
            Assert.AreEqual(expected.CreationTime, expected.CreationTime);
            Assert.AreEqual(expected.ModificationTime, expected.ModificationTime);

            Assert.AreEqual(expected.Tags?.Length, expected.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], expected.Tags[i]);
            }
        }

        void StructEqual(Struct expected, Struct real)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Name, real.Name);
            Assert.AreEqual(expected.CreationTime, real.CreationTime);
            Assert.AreEqual(expected.ModificationTime, real.ModificationTime);
            Assert.AreEqual(expected.Tags?.Length, real.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], real.Tags[i]);
            }
            Assert.AreEqual(expected.Data?.Length, real.Data?.Length);
            for (int i = 0; i < expected.Data?.Length; i++)
            {
                Assert.AreEqual(expected.Data[i].Ids?.Length, real.Data[i].Ids?.Length);
                for (int j = 0; j < expected.Data[i].Ids?.Length; j++)
                {
                    Assert.AreEqual(expected.Data[i].Ids[j], real.Data[i].Ids[j]);
                }
            }
        }

        [TestMethod]
        public void NodeData()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db");
            var options = builder.Options;

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                IDataProvider provider = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context);

                var controller = new NodesController(provider);
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                var tn = SampleNode();
                int id = controller.Create(tn).Result;
                Assert.IsTrue(id > 0);

                NodeEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                NodeEqual(tn, controller.GetAll().Result.First());

                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn));
                NodeEqual(tn, controller.Get(id).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }

        [TestMethod]
        public void StructData()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db");
            var options = builder.Options;

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                IDataProvider provider = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context);

                var controller = new StructsController(provider);
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                var tn = SampleStruct();
                int id = controller.Create(tn).Result;
                Assert.IsTrue(id > 0);

                StructEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                StructEqual(tn, controller.GetAll().Result.First());

                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn));
                StructEqual(tn, controller.Get(id).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }
    }
}
