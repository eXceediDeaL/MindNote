using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Server.API.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.API
{
    [TestClass]
    public class ServerSide
    {
        [TestMethod]
        public void Helper()
        {
            var cont = new HelpersController();
            cont.Heartbeat().Wait();
        }
    }
}
