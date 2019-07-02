using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data.Providers.SqlServer;
using MindNote.Data.Providers.SqlServer.Models;

namespace Test.Data
{
    [TestClass]
    public class SqlServerProvider
    {
        private DataContext CreateContext(string dbname)
        {
            DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbname)
                .Options;

            DataContext context = new DataContext(options);

            return context;
        }

        [TestMethod]
        public void Node()
        {
            using (DataContext context = CreateContext("sqlserver_test_node"))
            {
                Tester tester = new Tester(new DataProvider(context));
                tester.NodeIndependent();
            }
        }

        [TestMethod]
        public void Relation()
        {
            using (DataContext context = CreateContext("sqlserver_test_relation"))
            {
                Tester tester = new Tester(new DataProvider(context));
                tester.RelationIndependent();
            }
        }

        [TestMethod]
        public void Tag()
        {
            using (DataContext context = CreateContext("sqlserver_test_tag"))
            {
                Tester tester = new Tester(new DataProvider(context));
                tester.TagIndependent();
            }
        }
    }
}
