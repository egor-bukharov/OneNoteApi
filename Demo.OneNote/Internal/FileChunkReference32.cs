using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FileChunkReference32
    {
        public uint Offset;
        public uint Size;

        public override string ToString()
        {
            if (Size == 0)
            {
                return Offset == 0 ? "Zero" : "Nil";
            }

            return string.Format("Offset: {0}, Size: {1}", Offset, Size);
        }
    }
}
