using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using imagesIPM.Helpers;

namespace AppUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        [TestMethod]
        public void TestSupportedFiles()
        {
            var ok = SupportedFiles.IsSupported(".jpg");
            Assert.IsTrue(ok);

            var notok = SupportedFiles.IsSupported("somestring");
            Assert.IsFalse(notok);
        }




        [TestMethod]
        public void TestPathManager()
        {
            var pm = new PathsManager();
            for (var i = 0; i < 30; i++)
            {
                pm.PushPath(i.ToString());
            }

            Assert.AreEqual(10,pm.list().Count);
            Assert.AreEqual("29", pm.list().Last());
            Assert.AreEqual("20", pm.list().First());

        }
    }
}
