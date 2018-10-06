using System;
using System.IO;
using Demo.OneNote.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.IntegrationTests.Internal
{
    [TestClass]
    public class SectionReaderIntegrationTests
    {
        [TestMethod]
        public void ShouldLoadHeaderMembersProperly()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";
            
            const uint expectedBnCreated = 0;
            const uint expectedBnLastWroteToThisFile = 0;
            const uint expectedBnNewestWritten = 0;
            const uint expectedCrcName = 0xA944CC5C;
            const uint expectedCTransactionsInLog = 23;

            var expectedGuidFile = new Guid("{3fb6fa02-bd3b-4f24-a393-1965548f4f77}");
            var expectedGuidAncestor = new Guid("{cb44d324-1b38-4f25-9775-04fc94fe44f0}");
            
            var expectedFcrHashedChunkList = new FileChunkReference64x32 { Offset = 0x40000000000, Size = 2048};
            var expectedFcrTransactionLog  = new FileChunkReference64x32 {Offset = 0x40000000968, Size = 0};

            using (var stream = File.OpenRead(relativePathToInputFile))
            using (var reader = new BinaryReader(stream))
            {
                var sectionReader = new SectionReader();

                var header = new Header();
                sectionReader.ReadHeader(reader, ref header);

                Assert.AreEqual(SectionReader.FileTypeOne, header.guidFileType, "guidFileType");
                Assert.AreEqual(SectionReader.FileFormatConstant, header.guidFileFormat, "guidFileFormat");
                Assert.AreEqual(expectedGuidFile, header.guidFile, "guidFileFormat");
                Assert.AreEqual(FileFormat.One, header.ffvOldestCodeThatHasWrittenToThisFile, "ffvOldestCodeThatHasWrittenToThisFile");
                Assert.AreEqual(FileFormat.One, header.ffvNewestCodeThatHasWrittenToThisFile, "ffvNewestCodeThatHasWrittenToThisFile");
                Assert.AreEqual(FileFormat.One, header.ffvOldestCodeThatMayReadThisFile, "ffvOldestCodeThatMayReadThisFile");
                Assert.AreEqual(FileChunkReference32.Zero, header.fcrLegacyFreeChunkList, "fcrLegacyFreeChunkList");
                Assert.AreEqual(expectedCTransactionsInLog, header.cTransactionsInLog, "cTransactionsInLog");
                Assert.AreEqual(FileChunkReference32.Nil, header.fcrLegacyFileNodeListRoot, "fcrLegacyFileNodeListRoot");
                Assert.AreEqual(expectedGuidAncestor, header.guidAncestor, "guidAncestor");
                Assert.AreEqual(expectedCrcName, header.crcName, "crcName");
                Assert.AreEqual(expectedFcrHashedChunkList, header.fcrHashedChunkList, "fcrHashedChunkList");
                Assert.AreEqual(expectedFcrTransactionLog, header.fcrTransactionLog, "fcrTransactionLog");
                
                Assert.AreEqual(expectedBnCreated, header.bnCreated, "bnCreated");
                Assert.AreEqual(expectedBnLastWroteToThisFile, header.bnLastWroteToThisFile, "bnLastWroteToThisFile");
                Assert.AreEqual(expectedBnNewestWritten, header.bnNewestWritten, "bnNewestWritten");

                Assert.AreEqual(FileChunkReference64x32.Zero, header.fcrAllocVerificationFreeChunkList, "fcrAllocVerificationFreeChunkList");

                //Assert.AreEqual((ulong)stream.Length, header.cbExpectedFileLength, "cbExpectedFileLength");
            }
        }
    }
}
