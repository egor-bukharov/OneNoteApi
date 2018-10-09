using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileChunkReference16x8 : IFileChunkReference
    {
        public ushort compressedStp;
        public byte compressedCb;

        public static readonly uint SizeInBytes = (uint) Marshal.SizeOf(typeof(FileChunkReference16x8));

        public long Offset
        {
            get { return compressedStp * 8; }
        }

        public uint DataSize
        {
            get { return compressedCb * (uint)8; }
        }

        public uint SizeOfReferenceStructure
        {
            get { return SizeInBytes; }
        }
    }
}
