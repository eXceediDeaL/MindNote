using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data.Providers.SqlServer;
using MindNote.Data.Providers.SqlServer.Models;
using Test.Shared;

namespace Test.Data
{
    [TestClass]
    public class SqlServerProvider
    {
        [TestMethod]
        public void Note()
        {
            using (DataContext context = DBHelper.CreateContext<DataContext>("sqlserver_test_note"))
            {
                Tester tester = new Tester(new DataRepository(context));
                tester.NoteIndependent();
            }
        }

        [TestMethod]
        public void Category()
        {
            using (DataContext context = DBHelper.CreateContext<DataContext>("sqlserver_test_category"))
            {
                Tester tester = new Tester(new DataRepository(context));
                tester.CategoryIndependent();
            }
        }

        [TestMethod]
        public void User()
        {
            using (DataContext context = DBHelper.CreateContext<DataContext>("sqlserver_test_category"))
            {
                Tester tester = new Tester(new DataRepository(context));
                tester.UserIndependent();
            }
        }
    }
}
