using System.IO;

namespace Demo.OneNote.Internal
{
    internal struct FileChunkReference64x32
    {
        public ulong Offset;
        public uint Size;

        public override string ToString()
        {
            if (Size == 0)
            {
                return Offset == 0L ? "Zero" : "Nil";
            }

            return string.Format("Offset: {0}, Size: {1}", Offset, Size);
        }
    }
}
