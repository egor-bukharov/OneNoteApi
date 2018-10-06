using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileChunkReference64x32
    {
        private const ulong NilOffset = 0xFFFFFFFFFFFFFFFF;

        public ulong Offset;
        public uint Size;

        public override string ToString()
        {
            if (Size == 0)
            {
                if (Offset == 0)
                {
                    return "Zero";
                }

                if (Offset == NilOffset)
                {
                    return "Nil";
                }
            }

            return string.Format("Offset: {0}, Size: {1}", Offset, Size);
        }

        public static FileChunkReference64x32 Zero
        {
            get { return new FileChunkReference64x32(); }
        }

        public static FileChunkReference64x32 Nil
        {
            get { return new FileChunkReference64x32 { Offset = NilOffset, Size = 0 }; }
        }
    }
}
