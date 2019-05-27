using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Unit
{
    [TestClass]
    public class Integration
    {
        TestServer testServer;

        [TestInitialize]
        public void Init()
        {
            var host = MindNote.API.Program.CreateWebHostBuilder(new string[]{
                $"ConnectionString={MindNote.Data.Providers.SqlServer.DataContextFactory.LocalConnection}"
            }).UseStartup<MindNote.API.Startup>();
            testServer = new TestServer(host);
            MindNote.API.Program.InitialDatabase(testServer.Host);
        }

        [TestMethod]
        public void Nodes()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Nodes/Full").Result);
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Nodes/1/Full").Result);
                Assert.AreEqual("[]", client.GetStringAsync("/api/Nodes/1/Tags").Result);
            }
        }

        [TestMethod]
        public void Structs()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Structs/Full").Result);
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Nodes/1/Full").Result);
                Assert.AreEqual("[]", client.GetStringAsync("/api/Structs/1/Tags").Result);
            }
        }

        [TestMethod]
        public void Tags()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Tags/All").Result);
            }
        }

        [TestMethod]
        public void Relations()
        {
            using (var client = testServer.CreateClient())
            {
                Assert.AreNotEqual("[]", client.GetStringAsync("/api/Relations/All").Result);
            }
        }
    }
}
