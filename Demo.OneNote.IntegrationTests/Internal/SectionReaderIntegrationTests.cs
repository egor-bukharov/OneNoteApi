using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Demo.OneNote.Exceptions;
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

            var expectedFcrHashedChunkList = new FileChunkReference64x32 { stp = 7888, cb = 1024 };
            var expectedFcrTransactionLog = new FileChunkReference64x32 { stp = 2048, cb = 2408 };
            var expectedFcrFileNodeListRoot = new FileChunkReference64x32 { stp = 1024, cb = 1024 };

            // TODO: Move initializers and finalizers to a set-up method
            using (var stream = File.OpenRead(relativePathToInputFile))
            using (var reader = new BinaryReader(stream))
            {
                var sectionReader = new SectionReader(reader);

                var header = new Header();
                sectionReader.ReadHeader(ref header);

                Assert.AreEqual(FileTypeGuids.FileTypeOne, header.guidFileType);
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

        [TestMethod]
        public void ShouldReadTransactionLogProperly()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";

            using (var stream = File.OpenRead(relativePathToInputFile))
            using (var reader = new BinaryReader(stream))
            {
                var sectionReader = new SectionReader(reader);

                var header = new Header();
                sectionReader.ReadHeader(ref header);

                var transactionsData = new Dictionary<uint, uint>();

                sectionReader.Move(header.fcrTransactionLog.stp);
                sectionReader.ReadTransactionLog(transactionsData, header.cTransactionsInLog, header.fcrTransactionLog.cb);

                Assert.AreEqual(15, transactionsData.Count);

                // TODO: Add validators for all the transactions data items
                Assert.AreEqual((uint) 3, transactionsData[16]);
                Assert.AreEqual((uint) 37, transactionsData[30]);
            }
        }

        [TestMethod]
        public void ShouldLoadRootFileNodesListProperly()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";

            using (var stream = File.OpenRead(relativePathToInputFile))
            using (var reader = new BinaryReader(stream))
            {
                var sectionReader = new SectionReader(reader);

                var header = new Header();
                sectionReader.ReadHeader(ref header);

                // TODO: Enclose FileNodeListFragment enumeration into a loop
                var fileNodeListFragmentHeader = new FileNodeListHeader();
                sectionReader.ReadFileNodeListHeader(header.fcrFileNodeListRoot, ref fileNodeListFragmentHeader);

                Assert.AreEqual((uint) 0, fileNodeListFragmentHeader.nFragmentSequence);
                Assert.AreEqual(SectionReader.FileNodeListFragmentHeaderMagic, fileNodeListFragmentHeader.uintMagic);
                Assert.AreEqual((uint) 0x10, fileNodeListFragmentHeader.FileNodeListID);

                var fileNodeHeader = new FileNodeHeader();
                while (true)
                {
                    if (!sectionReader.ReadFileNodeHeader(ref fileNodeHeader))
                    {
                        throw new FileFormatException("Cannot read header of FileNode");
                    }

                    if (fileNodeHeader.FileNodeID == FileNodeIDs.ChunkTerminatorFND)
                    {
                        break;
                    }

                    //if (fileNodeHeader.FileNodeID != FileNodeIDs.ObjectSpaceManifestListReferenceFND)
                    //{
                    //    sectionReader.Skip(fileNodeHeader.Size);
                    //}

                    sectionReader.Skip(fileNodeHeader.Size - (uint) Marshal.SizeOf(fileNodeHeader));

                    break;
                }
            }
        }
    }
}
