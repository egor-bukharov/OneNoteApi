using System.Collections.Generic;
using System.IO;
using Demo.OneNote.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.IntegrationTests.Internal
{
    [TestClass]
    public class SectionReaderIntegrationTests
    {
        private Stream stream;
        private BinaryReader binaryReader;
        private OneNoteFileReader oneNoteFileReader;
        private SectionReader sectionReader;

        [TestInitialize]
        public void BeforeEachTest()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";

            stream = File.OpenRead(relativePathToInputFile);
            binaryReader = new BinaryReader(stream);
            oneNoteFileReader = new OneNoteFileReader(binaryReader);
            sectionReader = new SectionReader(oneNoteFileReader);
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            binaryReader.Dispose();
            stream.Dispose();
        }

        [TestMethod]
        public void ShouldReadTransactionLogProperly()
        {
            var header = new Header();
            oneNoteFileReader.ReadHeader(ref header);

            var transactionsData = new Dictionary<uint, uint>();

            sectionReader.ReadTransactionLog(transactionsData, header.fcrTransactionLog, header.cTransactionsInLog);

            Assert.AreEqual(15, transactionsData.Count);

            // TODO: Add validators for all the transactions data items
            Assert.AreEqual((uint)3, transactionsData[16]);
            Assert.AreEqual((uint)37, transactionsData[30]);
        }

        [TestMethod]
        public void ShouldLoadRootFileNodesListProperly()
        {
            var header = new Header();
            oneNoteFileReader.ReadHeader(ref header);

            var transactionsData = new Dictionary<uint, uint>();
            sectionReader.ReadTransactionLog(transactionsData, header.fcrTransactionLog, header.cTransactionsInLog);

            var objectSpaceManifestListNodes = sectionReader.ReadFileNodeList(header.fcrFileNodeListRoot, transactionsData, FileNodeIDs.ObjectSpaceManifestListReferenceFND);
            foreach (var objectSpaceManifestListNode in objectSpaceManifestListNodes)
            {
                var objectSpaceManifestListReferenceFnd = (ObjectSpaceManifestListReferenceFND) objectSpaceManifestListNode;
                var revisionManifestListNodes = sectionReader.ReadFileNodeList(objectSpaceManifestListReferenceFnd.fileChunkReference, transactionsData, FileNodeIDs.RevisionManifestListReferenceFND);

                foreach (var revisionManifestListNode in revisionManifestListNodes)
                {
                    var revisionManifestListReferenceFnd = (RevisionManifestListReferenceFND) revisionManifestListNode;
                    var objectGroupListNodes = sectionReader.ReadFileNodeList(
                        revisionManifestListReferenceFnd.fileChunkReference, transactionsData,
                        FileNodeIDs.ObjectGroupListReferenceFND);

                    foreach (var objectGroupNode in objectGroupListNodes)
                    {
                        var groupListReferenceFnd = (ObjectGroupListReferenceFND) objectGroupNode;
                        var objectDeclaration2RefCountNodes = sectionReader.ReadFileNodeList(groupListReferenceFnd.fileChunkReference, transactionsData, FileNodeIDs.ObjectDeclaration2RefCountFND);
                        foreach (var objectDeclaration2RefCountNode in objectDeclaration2RefCountNodes)
                        {
                            var objectDeclaration2RefCountFnd = (ObjectDeclaration2RefCountFND) objectDeclaration2RefCountNode;
                            var propSet = sectionReader.ReadObjectSpaceObjectPropSet(objectDeclaration2RefCountFnd.blobFileChunkReference);
                        }
                    }
                }
            }

        }
    }
}
