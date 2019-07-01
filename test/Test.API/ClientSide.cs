using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Client.API;

namespace Test.API
{
    [TestClass]
    public class ClientSide
    {
        [TestMethod]
        public void Helper()
        {
            var cont = new HelpersClient(null);
        }
    }
}
