using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Unit
{
    [TestClass]
    public class Basic
    {
        [TestMethod]
        public void Test()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
