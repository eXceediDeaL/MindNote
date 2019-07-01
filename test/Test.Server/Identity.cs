using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Server.Identity;
using System;

namespace Test.Server
{
    [TestClass]
    public class Identity
    {
        [TestMethod]
        public void Test()
        {
            using (TestServer testServer = new TestServer(Program.CreateWebHostBuilder(Array.Empty<string>())))
            {

            }
        }
    }
}
