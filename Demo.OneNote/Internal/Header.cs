using System;
using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    // TODO: Members that must be ignored were made private and readonly

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Header
    {
        public Guid guidFileType;
        public Guid guidFile;
        private readonly Guid guidLegacyFileVersion; // must be zero and must be ignored
        public Guid guidFileFormat;
        public FileFormat ffvLastCodeThatWroteToThisFile;
        public FileFormat ffvOldestCodeThatHasWrittenToThisFile;
        public FileFormat ffvNewestCodeThatHasWrittenToThisFile;
        public FileFormat ffvOldestCodeThatMayReadThisFile;
        public FileChunkReference32 fcrLegacyFreeChunkList; // must be zero
        public FileChunkReference32 fcrLegacyTransactionLog; // must be nil
        public uint cTransactionsInLog; // must not be zero
        private readonly uint cbLegacyExpectedFileLength; // must be zero and must be ignored
        private readonly ulong rgbPlaceholder; // must be zero and must be ignored
        public FileChunkReference32 fcrLegacyFileNodeListRoot; // must be nil
        private readonly uint cbLegacyFreeSpaceInFreeChunkList; // must be zero and must be ignored
        private readonly byte fNeedsDefrag; // must be ignored
        private readonly byte fRepairedFile; // must be ignored
        private readonly byte fNeedsGarbageCollect; // must be ignored
        private readonly byte fHasNoEmbeddedFileObjects; // must be ignored
        public Guid guidAncestor;
        public uint crcName; 
        public FileChunkReference64x32 fcrHashedChunkList; // can be zero or nil
        public FileChunkReference64x32 fcrTransactionLog; // must not be zero or nil
        public FileChunkReference64x32 fcrFileNodeListRoot; // must not be zero or nil
        public FileChunkReference64x32 fcrFreeChunkList; // must not be zero or nil
        public ulong cbExpectedFileLength;
        public ulong cbFreeSpaceInFreeChunkList;
        public Guid guidFileVersion;
        public ulong nFileVersionGeneration; // must be incremented when the guidFileVersion field changes
        public Guid guidDenyReadFileVersion;
        private readonly uint grfDebugLogFlags; // must be zero and must be ignored
        private readonly FileChunkReference64x32 fcrDebugLog; // must be zero and must be ignored
        private readonly FileChunkReference64x32 fcrAllocVerificationFreeChunkList; // must be zero and must be ignored
        private readonly uint bnCreated; // should be ignored
        private readonly uint bnLastWroteToThisFile; // should be ignored
        private readonly uint bnNewestWritten; // should be ignored
    }
}
