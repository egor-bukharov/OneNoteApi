using System;
using System.IO;
using Demo.OneNote.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.IntegrationTests.Internal
{
    [TestClass]
    public class OneNoteFileReaderIntegrationTests
    {
        private Stream stream;
        private BinaryReader binaryReader;
        private OneNoteFileReader oneNoteFileReader;

        [TestInitialize]
        public void BeforeEachTest()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";

            stream = File.OpenRead(relativePathToInputFile);
            binaryReader = new BinaryReader(stream);
            oneNoteFileReader = new OneNoteFileReader(binaryReader);
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            binaryReader.Dispose();
            stream.Dispose();
        }

        [TestMethod]
        public void ShouldLoadHeaderMembersProperly()
        {
            const uint expectedCrcName = 0xA944CC5C;
            const uint expectedCTransactionsInLog = 23;
            const ulong expectedCbFreeSpaceInFreeChunkList = 0x30;
            const ulong expectedNFileVersionGeneration = 113;

            var expectedGuidFile = new Guid("{3fb6fa02-bd3b-4f24-a393-1965548f4f77}");
            var expectedGuidAncestor = new Guid("{cb44d324-1b38-4f25-9775-04fc94fe44f0}");
            var expectedGuidDenyReadFileVersion = new Guid("{bbeab090-ef4e-49ef-81e3-1ab43721f73c}");

            var expectedFcrHashedChunkList = new FileChunkReference64x32 { stp = 7888, cb = 1024 };
            var expectedFcrTransactionLog = new FileChunkReference64x32 { stp = 2048, cb = 2408 };
            var expectedFcrFileNodeListRoot = new FileChunkReference64x32 { stp = 1024, cb = 1024 };

            var header = new Header();
            oneNoteFileReader.ReadHeader(ref header);

            Assert.AreEqual(FileTypeGuids.FileTypeOne, header.guidFileType);
            Assert.AreEqual(OneNoteFileReader.FileFormatConstant, header.guidFileFormat);
            Assert.AreEqual(expectedGuidFile, header.guidFile);
            Assert.AreEqual(FileFormat.One, header.ffvOldestCodeThatHasWrittenToThisFile);
            Assert.AreEqual(FileFormat.One, header.ffvNewestCodeThatHasWrittenToThisFile);
            Assert.AreEqual(FileFormat.One, header.ffvOldestCodeThatMayReadThisFile);
            Assert.AreEqual(FileChunkReference32.Zero, header.fcrLegacyFreeChunkList);
            Assert.AreEqual(expectedCTransactionsInLog, header.cTransactionsInLog);
            Assert.AreEqual(FileChunkReference32.Nil, header.fcrLegacyFileNodeListRoot);
            Assert.AreEqual(expectedGuidAncestor, header.guidAncestor);
            Assert.AreEqual(expectedCrcName, header.crcName);
            Assert.AreEqual(expectedFcrHashedChunkList, header.fcrHashedChunkList);
            Assert.AreEqual(expectedFcrTransactionLog, header.fcrTransactionLog);
            Assert.AreEqual(expectedFcrFileNodeListRoot, header.fcrFileNodeListRoot);
            Assert.AreEqual(FileChunkReference64x32.Nil, header.fcrFreeChunkList);
            Assert.AreEqual((ulong)stream.Length, header.cbExpectedFileLength);
            Assert.AreEqual(expectedCbFreeSpaceInFreeChunkList, header.cbFreeSpaceInFreeChunkList);
            Assert.AreNotEqual(Guid.Empty, header.guidFileVersion);
            Assert.AreEqual(expectedNFileVersionGeneration, header.nFileVersionGeneration);
            Assert.AreEqual(expectedGuidDenyReadFileVersion, header.guidDenyReadFileVersion);
        }
    }
}
