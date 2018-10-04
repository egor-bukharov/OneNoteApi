using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Header
    {
        public Guid FileType;
        public Guid File;
        private readonly Guid LegacyFileVersion; // must be zero and must be ignored
        public Guid FileFormat;
        public FileFormat LastCodeThatWroteToThisFile;
        public FileFormat OldestCodeThatHasWrittenToThisFile;
        public FileFormat NewestCodeThatHasWrittenToThisFile;
        public FileFormat OldestCodeThatMayReadThisFile;
        public FileChunkReference32 LegacyFreeChunkList; // must be zero
        public FileChunkReference32 LegacyTransactionLog; // must be nil
        public uint TransactionsInLog; // must not be zero
        private readonly uint LegacyExpectedFileLength; // must be zero and must be ignored
        private readonly ulong Placeholder; // must be zero and must be ignored
        public FileChunkReference32 LegacyFileNodeListRoot; // must be nil
        private readonly uint LegacyFreeSpaceInFreeChunkList; // must be zero and must be ignored
        private readonly byte NeedsDefrag; // must be ignored
        private readonly byte RepairedFile; // must be ignored
        private readonly byte NeedsGarbageCollect; // must be ignored
        private bool HasNoEmbeddedFileObjects; // must be ignored
        public Guid Ancestor;
        public uint NameCRC;
    }
}
