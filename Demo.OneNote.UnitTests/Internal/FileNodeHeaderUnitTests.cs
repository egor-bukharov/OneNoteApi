using Demo.OneNote.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.UnitTests.Internal
{
    [TestClass]
    public class FileNodeHeaderUnitTests
    {
        private class TestCase
        {
            private readonly uint fileNodeID;
            private readonly uint size;
            private readonly StpFormat stpFormat;
            private readonly CbFormat cbFormat;

            private readonly uint value;

            public TestCase(uint fileNodeId, uint size, StpFormat stpFormat, CbFormat cbFormat, uint value)
            {
                fileNodeID = fileNodeId;
                this.size = size;
                this.stpFormat = stpFormat;
                this.cbFormat = cbFormat;
                this.value = value;
            }

            public void Validate()
            {
                var fileNodeHeader = new FileNodeHeader { Value = value };

                Assert.AreEqual(value, fileNodeHeader.Value);

                Assert.AreEqual(fileNodeID, fileNodeHeader.FileNodeID);
                Assert.AreEqual(size, fileNodeHeader.Size);
                Assert.AreEqual(stpFormat, fileNodeHeader.StpFormat);
                Assert.AreEqual(cbFormat, fileNodeHeader.CbFormat);
            }
        }

        [TestMethod]
        public void ShouldApplyBitMaskProperly()
        {
            var testCases = new[]
            {
                new TestCase(0x3FF, 0, (StpFormat) 0, (CbFormat) 0, 0x3FF),
                new TestCase(0, 0x1FFF, (StpFormat) 0, (CbFormat) 0, 0x7FFC00),
                new TestCase(0, 0, (StpFormat) 3, (CbFormat) 0, 0x1800000),
                new TestCase(0, 0, (StpFormat) 0, (CbFormat) 3, 0x6000000),
                new TestCase(0x3FF, 0x1FFF, (StpFormat) 0, (CbFormat) 0, 0x7FFFFF),
                new TestCase(0x3FF, 0x1FFF, (StpFormat) 3, (CbFormat) 0, 0x1FFFFFF),
                new TestCase(0x3FF, 0x1FFF, (StpFormat) 3, (CbFormat) 3, 0x7FFFFFF),
            };

            foreach (var testCase in testCases)
            {
                testCase.Validate();
            }
        }
    }
}
