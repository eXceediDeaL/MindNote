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
            var tester = new Tester(new DataProvider());
            tester.NodeIndependent();
        }

        [TestMethod]
        public void Relatioin()
        {
            var tester = new Tester(new DataProvider());
            tester.RelationIndependent();
        }

        [TestMethod]
        public void Tag()
        {
            var tester = new Tester(new DataProvider());
            tester.TagIndependent();
        }
    }
}
