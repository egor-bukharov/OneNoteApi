using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileChunkReference32
    {
        private const uint NilOffset = 0xFFFFFFFF;

        public uint Offset;
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

        public static FileChunkReference32 Zero
        {
            get { return new FileChunkReference32(); }
        }

        public static FileChunkReference32 Nil
        {
            get { return new FileChunkReference32 {Offset = NilOffset, Size = 0}; }
        }
    }
}
