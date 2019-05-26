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
        

        void NodeEqual(Node expected, Node real, bool ignore = false)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Name, real.Name);
            if (!ignore) Assert.AreEqual(expected.Content, real.Content);

            Assert.AreEqual(expected.Tags?.Length, real.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], real.Tags[i]);
            }
        }

        void StructEqual(Struct expected, Struct real, bool ignore = false)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Name, real.Name);
            Assert.AreEqual(expected.Tags?.Length, real.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], real.Tags[i]);
            }
            if (!ignore)
            {
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
        }

        [TestMethod]
        public void Node()
        {
            var tn = Helper.SampleNode();

            var ctn = MindNote.Data.Providers.SqlServer.Models.Node.FromModel(tn).ToModel();

            NodeEqual(tn, ctn);
        }

        [TestMethod]
        public void Struct()
        {
            var tn = Helper.SampleStruct();
            var ctn = MindNote.Data.Providers.SqlServer.Models.Struct.FromModel(tn).ToModel();

            StructEqual(tn, ctn);
        }

        [TestMethod]
        public void NodeData()
        {
            var builder = new DbContextOptionsBuilder<MindNote.Data.Providers.SqlServer.Models.DataContext>();
            builder.UseInMemoryDatabase("db_node");
            var options = builder.Options;

            var tn = Helper.SampleNode();

            using (var context = new MindNote.Data.Providers.SqlServer.Models.DataContext(options))
            {
                var controller = new MindNote.Data.Providers.SqlServer.SqlServerProvider(context).GetNodesProvider();
                
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

                NodeEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                NodeEqual(tn, controller.GetAll().Result.First(), true);
                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                NodeEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }

        [TestMethod]
        public void StructData()
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

                StructEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                StructEqual(tn, controller.GetAll().Result.First(), true);

                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                StructEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }
    }
}
