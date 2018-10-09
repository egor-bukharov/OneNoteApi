using System;
using System.Collections.Generic;
using Demo.OneNote.Exceptions;

namespace Demo.OneNote.Internal
{
    public class SectionReader
    {
        private readonly OneNoteFileReader oneNoteFileReader;

        public SectionReader(OneNoteFileReader oneNoteFileReader)
        {
            this.oneNoteFileReader = oneNoteFileReader;
        }

        public Section ReadSection()
        {
            var header = new Header();
            oneNoteFileReader.ReadHeader(ref header);

            var transactionsData = new Dictionary<uint, uint>();

            ReadTransactionLog(transactionsData, header.fcrTransactionLog, header.cTransactionsInLog);

            var objectSpaceManifestListNodes = ReadFileNodeList(header.fcrFileNodeListRoot, transactionsData, FileNodeIDs.ObjectSpaceManifestListReferenceFND);
            foreach (var objectSpaceManifestListNode in objectSpaceManifestListNodes)
            {
                var objectSpaceManifestListReferenceFnd = (ObjectSpaceManifestListReferenceFND)objectSpaceManifestListNode;
                var revisionManifestListNodes = ReadFileNodeList(objectSpaceManifestListReferenceFnd.fileChunkReference, transactionsData, FileNodeIDs.RevisionManifestListReferenceFND);

                foreach (var revisionManifestListNode in revisionManifestListNodes)
                {
                    var revisionManifestListReferenceFnd = (RevisionManifestListReferenceFND)revisionManifestListNode;
                    var objectGroupListNodes = ReadFileNodeList(
                        revisionManifestListReferenceFnd.fileChunkReference, transactionsData,
                        FileNodeIDs.ObjectGroupListReferenceFND);

                    foreach (var objectGroupNode in objectGroupListNodes)
                    {
                        var groupListReferenceFnd = (ObjectGroupListReferenceFND)objectGroupNode;
                        var objectDeclaration2RefCountNodes = ReadFileNodeList(groupListReferenceFnd.fileChunkReference, transactionsData, FileNodeIDs.ObjectDeclaration2RefCountFND);
                        foreach (var objectDeclaration2RefCountNode in objectDeclaration2RefCountNodes)
                        {
                            var objectDeclaration2RefCountFnd = (ObjectDeclaration2RefCountFND)objectDeclaration2RefCountNode;
                            //if (objectDeclaration2RefCountFnd.body.jcid.value == 0x00060007) // section
                            //{
                            //    var propSet = ReadObjectSpaceObjectPropSet(objectDeclaration2RefCountFnd.blobFileChunkReference);
                            //}
                        }
                    }
                }
            }

            return new Section();
        }

        public void ReadTransactionLog(Dictionary<uint, uint> transactionsData, IFileChunkReference fileChunkReference,
            uint cTransactionsInLog)
        {
            oneNoteFileReader.Move(fileChunkReference.Offset, "TransactionLog");

            uint cbRead = 0;
            var cbTransactionLogFragmentSize = fileChunkReference.DataSize;
            var cbMax = cbTransactionLogFragmentSize - FileChunkReference64x32.SizeInBytes;

            for (var i = 0; i < cTransactionsInLog; i++)
            {
                if (cbRead == cbMax)
                {
                    const string operationContext = "TransactionLog.nextFragment";

                    var nextFragment = new FileChunkReference64x32();
                    
                    oneNoteFileReader.ReadFileChunkReference64x32(ref nextFragment, operationContext);
                    oneNoteFileReader.Move(nextFragment.Offset, operationContext);

                    cbRead = 0;
                    cbMax = nextFragment.cb - FileChunkReference64x32.SizeInBytes;
                }
                else if (cbRead > cbMax)
                {
                    throw new FileFormatException("Transaction log is broken");
                }

                ReadTransaction(transactionsData, ref cbRead);
            }
        }

        public void ReadTransaction(Dictionary<uint, uint> transactionsData, ref uint cb)
        {
            var transactionEntry = new TransactionEntry();
            while (true)
            {
                oneNoteFileReader.ReadTransactionEntry(ref transactionEntry);
                cb += TransactionEntry.SizeInBytes;

                if (transactionEntry.srcID == TransactionEntry.SentinelEntryId)
                {
                    break;
                }

                transactionsData[transactionEntry.srcID] = transactionEntry.TransactionEntrySwitch;
            }
        }

        public ICollection<FNDBase> ReadFileNodeList(IFileChunkReference fileChunkReference, Dictionary<uint, uint> transactionsData, params uint[] fileNodeIds)
        {
            ICollection<FNDBase> result = new List<FNDBase>();

            var nextFragment = new FileChunkReference64x32();
            while (true)
            {
                ReadFileNodeListFragment(fileChunkReference, result, ref nextFragment, transactionsData, fileNodeIds);
                
                if (FileChunkReference64x32.Nil.Equals(nextFragment))
                {
                    break;
                }

                fileChunkReference = nextFragment;
            }

            return result;
        }

        public void ReadFileNodeListFragment(IFileChunkReference fileChunkReference, ICollection<FNDBase> result, ref FileChunkReference64x32 nextFragment, Dictionary<uint, uint> transactionsData, params uint[] fileNodeIds)
        {
            oneNoteFileReader.Move(fileChunkReference.Offset, "FileNodeListFragment");

            var jumpSizeToNextFragmentReference = GetJumpSizeToNextFragmentReference(fileChunkReference.DataSize);

            var fileNodeListHeader = new FileNodeListHeader();
            oneNoteFileReader.ReadFileNodeListHeader(ref fileNodeListHeader);
            jumpSizeToNextFragmentReference -= FileNodeListHeader.SizeInBytes;

            var fileNodeListId = fileNodeListHeader.FileNodeListID;
            var nodesCount = transactionsData[fileNodeListId];
            var fileNodeHeader = new FileNodeHeader();

            while (true)
            {
                if (nodesCount == 0)
                {
                    break;
                }

                oneNoteFileReader.ReadFileNodeHeader(ref fileNodeHeader);
                jumpSizeToNextFragmentReference -= FileNodeHeader.SizeInBytes;

                var fileNodeId = fileNodeHeader.FileNodeID;
                if (fileNodeId == FileNodeIDs.ChunkTerminatorFND)
                {
                    break;
                }

                var sizeOfData = fileNodeHeader.Size - FileNodeHeader.SizeInBytes;
                var skip = true;

                if (fileNodeIds.Length == 0 || Array.IndexOf(fileNodeIds, fileNodeId) >= 0)
                {
                    var readFileNodeData = ReadFileNodeData(fileNodeHeader);
                    if (readFileNodeData == null)
                    {
                        readFileNodeData = new NotImplementedFND(fileNodeHeader);
                    }
                    else
                    {
                        skip = false;
                    }

                    result.Add(readFileNodeData);
                    jumpSizeToNextFragmentReference -= sizeOfData;
                }

                if (skip && sizeOfData != 0)
                {
                    oneNoteFileReader.Skip(sizeOfData);
                    jumpSizeToNextFragmentReference -= sizeOfData;
                }

                nodesCount--;

                const uint minJumpSize = 4;
                if (jumpSizeToNextFragmentReference < minJumpSize)
                {
                    break;
                }
            }

            if (jumpSizeToNextFragmentReference != 0)
            {
                oneNoteFileReader.Skip(jumpSizeToNextFragmentReference);
            }

            oneNoteFileReader.ReadFileChunkReference64x32(ref nextFragment, "FileNodeListFragment.nextFragment");
            oneNoteFileReader.ExpectFileNodeListFooter();

            transactionsData[fileNodeListId] = nodesCount;
        }

        public FNDBase ReadFileNodeData(FileNodeHeader header)
        {
            switch (header.FileNodeID)
            {
                case FileNodeIDs.ObjectSpaceManifestListReferenceFND:
                    return ReadObjectSpaceManifestListReferenceFND(header);
                case FileNodeIDs.RevisionManifestListReferenceFND:
                    return ReadRevisionManifestListReferenceFND(header);
                case FileNodeIDs.ObjectGroupListReferenceFND:
                    return ReadObjectGroupListReferenceFND(header);
                case FileNodeIDs.ObjectDeclaration2RefCountFND:
                    return ReadObjectDeclaration2RefCountFND(header);
                default:
                    return null;
            }
        }

        public ObjectSpaceManifestListReferenceFND ReadObjectSpaceManifestListReferenceFND(FileNodeHeader header)
        {
            var fileChunkReference = oneNoteFileReader.ReadFileChunkReference(header.StpFormat, header.CbFormat, "ObjectSpaceManifestListReferenceFND.ref");

            var gosid = new ExtendedGuid();
            oneNoteFileReader.ReadExtendedGuid(ref gosid, "ObjectSpaceManifestListReferenceFND.gosid");

            return new ObjectSpaceManifestListReferenceFND(header, fileChunkReference, gosid);
        }

        public RevisionManifestListReferenceFND ReadRevisionManifestListReferenceFND(FileNodeHeader header)
        {
            var fileChunkReference = oneNoteFileReader.ReadFileChunkReference(header.StpFormat, header.CbFormat, "RevisionManifestListReferenceFND.ref");

            return new RevisionManifestListReferenceFND(header, fileChunkReference);
        }

        public ObjectGroupListReferenceFND ReadObjectGroupListReferenceFND(FileNodeHeader header)
        {
            var fileChunkReference = oneNoteFileReader.ReadFileChunkReference(header.StpFormat, header.CbFormat, "ObjectSpaceManifestListReferenceFND.ref");

            var objectGroupID = new ExtendedGuid();
            oneNoteFileReader.ReadExtendedGuid(ref objectGroupID, "ObjectSpaceManifestListReferenceFND.objectGroupID");

            return new ObjectGroupListReferenceFND(header, fileChunkReference, objectGroupID);
        }

        public ObjectDeclaration2RefCountFND ReadObjectDeclaration2RefCountFND(FileNodeHeader header)
        {
            var fileChunkReference = oneNoteFileReader.ReadFileChunkReference(header.StpFormat, header.CbFormat, "ObjectDeclaration2RefCountFND.BlobRef");

            var body = new ObjectDeclaration2Body();
            oneNoteFileReader.ReadObjectDeclaration2Body(ref body);

            var cRef = oneNoteFileReader.ReadByte();

            return new ObjectDeclaration2RefCountFND(header, fileChunkReference, body, cRef);
        }

        public uint GetJumpSizeToNextFragmentReference(uint fileNodeListFragmentSize)
        {
            return fileNodeListFragmentSize - sizeof(ulong) - FileChunkReference64x32.SizeInBytes;
        }

        public ObjectSpaceObjectStream ReadObjectSpaceObjectStream()
        {
            var header = new ObjectSpaceObjectStreamHeader();
            oneNoteFileReader.ReadObjectSpaceObjectStreamHeader(ref header);

            var count = header.Count;
            var items = new List<CompactID>((int) count);

            for (var i = 0; i < count; i++)
            {
                items.Add(oneNoteFileReader.ReadCompactID());
            }

            return new ObjectSpaceObjectStream(header, items);
        }

        public ObjectSpaceObjectPropSet ReadObjectSpaceObjectPropSet(IFileChunkReference fileChunkReference)
        {
            oneNoteFileReader.Move(fileChunkReference.Offset, "PropertySet");

            var oids = ReadObjectSpaceObjectStream();
            var osids = oids.Header.OsidStreamNotPresent ? null : ReadObjectSpaceObjectStream();
            var contextIDs = oids.Header.ExtendedStreamsPresent ? ReadObjectSpaceObjectStream() : null;

            return new ObjectSpaceObjectPropSet(oids, osids, contextIDs);
        }
    }
}
