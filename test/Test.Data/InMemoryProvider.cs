using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data.Providers.InMemory;

namespace Test.Data
{
    [TestClass]
    public class InMemoryProvider
    {
        [TestMethod]
        public void Node()
        {
            Tester tester = new Tester(new DataProvider());
            tester.NodeIndependent();
        }

        [TestMethod]
        public void Relatioin()
        {
            Tester tester = new Tester(new DataProvider());
            tester.RelationIndependent();
        }

        [TestMethod]
        public void Tag()
        {
            Tester tester = new Tester(new DataProvider());
            tester.TagIndependent();
        }
    }
}
