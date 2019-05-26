using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Unit
{
    [TestClass]
    public class SeedData
    {
        TestServer testServer;

        [TestInitialize]
        public void Init()
        {
            var host = MindNote.API.Program.CreateWebHostBuilder(new string[]{
                $"ConnectionString={MindNote.Data.Providers.SqlServer.DataContextFactory.LocalConnection}"
            }).UseStartup<MindNote.API.Startup>();
            testServer = new TestServer(host);
        }

        [TestMethod]
        public void SeedInit()
        {
            MindNote.API.Program.InitialDatabase(testServer.Host);
        }
    }
}
