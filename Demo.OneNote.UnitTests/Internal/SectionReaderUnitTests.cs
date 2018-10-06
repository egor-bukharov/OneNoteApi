using System;
using Demo.OneNote.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.UnitTests.Internal
{
    [TestClass]
    public class SectionReaderUnitTests
    {
        [TestMethod]
        public void TestConstants()
        {
            Assert.AreEqual(new Guid("{109ADD3F-911B-49F5-A5D0-1791EDC8AED8}"), SectionReader.FileFormatConstant);

            Assert.AreEqual(new Guid("{7B5C52E4-D88C-4DA7-AEB1-5378D02996D3}"), SectionReader.FileTypeOne);
            Assert.AreEqual(new Guid("{43FF2FA1-EFD9-4C76-9EE2-10EA5722765F}"), SectionReader.FileTypeOnetoc2);
        }
    }
}
