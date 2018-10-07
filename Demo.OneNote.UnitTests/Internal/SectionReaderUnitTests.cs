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
            Assert.AreEqual(0xA4567AB1F5F7F4C4, SectionReader.FileNodeListFragmentHeaderMagic);
        }
    }
}
