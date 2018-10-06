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
            
            const uint expectedCrcName = 0xA944CC5C;
            const uint expectedCTransactionsInLog = 23;
            const ulong expectedCbFreeSpaceInFreeChunkList = 0x30;
            const ulong expectedNFileVersionGeneration = 113;

            var expectedGuidFile = new Guid("{3fb6fa02-bd3b-4f24-a393-1965548f4f77}");
            var expectedGuidAncestor = new Guid("{cb44d324-1b38-4f25-9775-04fc94fe44f0}");
            var expectedGuidDenyReadFileVersion = new Guid("{bbeab090-ef4e-49ef-81e3-1ab43721f73c}");

            var expectedFcrHashedChunkList = new FileChunkReference64x32 { Offset = 7888, Size = 1024 };
            var expectedFcrTransactionLog = new FileChunkReference64x32 { Offset = 2048, Size = 2408 };
            var expectedFcrFileNodeListRoot = new FileChunkReference64x32 { Offset = 1024, Size = 1024 };

            using (var stream = File.OpenRead(relativePathToInputFile))
            using (var reader = new BinaryReader(stream))
            {
                var sectionReader = new SectionReader();

                var header = new Header();
                sectionReader.ReadHeader(reader, ref header);

                Assert.AreEqual(SectionReader.FileTypeOne, header.guidFileType);
                Assert.AreEqual(SectionReader.FileFormatConstant, header.guidFileFormat);
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
}
