using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data.Providers;
using MindNote.Data.Providers.SqlServer;
using MindNote.Data.Providers.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data
{
    [TestClass]
    public class SqlServerProvider
    {

        DataContext CreateContext(string dbname)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbname)
                .Options;

            var context = new DataContext(options);

            return context;
        }

        [TestMethod]
        public void Node()
        {
            using(var context = CreateContext("sqlserver_test_node"))
            {
                var tester = new Tester(new DataProvider(context));
                tester.NodeIndependent();
            }
        }

        [TestMethod]
        public void Relation()
        {
            using (var context = CreateContext("sqlserver_test_relation"))
            {
                var tester = new Tester(new DataProvider(context));
                tester.RelationIndependent();
            }
        }

        [TestMethod]
        public void Tag()
        {
            using (var context = CreateContext("sqlserver_test_tag"))
            {
                var tester = new Tester(new DataProvider(context));
                tester.TagIndependent();
            }
        }
    }
}
