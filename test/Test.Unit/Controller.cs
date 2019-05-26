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
    public class Controller
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
                var controller = new NodesController(new MindNote.Data.Providers.SqlServer.SqlServerProvider(context));
                
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

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
                var controller = new StructsController(new MindNote.Data.Providers.SqlServer.SqlServerProvider(context));
                Assert.AreEqual(0, controller.GetAll().Result.Count());

                int id = controller.Create(tn).Result;

                Assert.IsTrue(id > 0);

                Helper.StructEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(1, controller.GetAll().Result.Count());
                Helper.StructEqual(tn, controller.GetAll().Result.First(), true);

                tn.Name = "updated";
                Assert.AreEqual(id, controller.Update(id, tn).Result);
                Helper.StructEqual(tn, controller.Get(id).Result);

                Assert.AreEqual(-1, controller.Update(0, null).Result);

                controller.Delete(id).Wait();

                Assert.AreEqual(0, controller.GetAll().Result.Count());

                controller.Delete(0).Wait();
            }
        }
    }
}
