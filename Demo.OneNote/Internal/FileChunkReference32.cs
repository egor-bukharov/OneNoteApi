using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileChunkReference32 : IFileChunkReference
    {
        public static readonly uint SizeInBytes = (uint) Marshal.SizeOf(typeof(FileChunkReference32));

        private const uint OffsetValueOfNil = 0xFFFFFFFF;

        public uint stp;
        public uint cb;

        public override string ToString()
        {
            if (cb == 0)
            {
                if (stp == 0)
                {
                    return "Zero";
                }

                if (stp == OffsetValueOfNil)
                {
                    return "Nil";
                }
            }

            return string.Format("stp: {0}, cb: {1}", stp, cb);
        }

        public static FileChunkReference32 Zero
        {
            get { return new FileChunkReference32(); }
        }

        public static FileChunkReference32 Nil
        {
            get { return new FileChunkReference32 {stp = OffsetValueOfNil, cb = 0}; }
        }

        public long Offset
        {
            get { return stp; }
        }

        public uint DataSize { get { return cb; } }

        public uint SizeOfReferenceStructure
        {
            get { return SizeInBytes; }
        }
    }
}
