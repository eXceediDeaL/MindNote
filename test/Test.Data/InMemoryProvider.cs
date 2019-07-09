using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data.Providers.InMemory;

namespace Test.Data
{
    [TestClass]
    public class InMemoryProvider
    {
        [TestMethod]
        public void Note()
        {
            Tester tester = new Tester(new DataProvider());
            tester.NoteIndependent();
        }

        [TestMethod]
        public void Relatioin()
        {
            Tester tester = new Tester(new DataProvider());
            tester.RelationIndependent();
        }

        [TestMethod]
        public void Category()
        {
            Tester tester = new Tester(new DataProvider());
            tester.CategoryIndependent();
        }

        [TestMethod]
        public void User()
        {
            Tester tester = new Tester(new DataProvider());
            tester.UserIndependent();
        }
    }
}
