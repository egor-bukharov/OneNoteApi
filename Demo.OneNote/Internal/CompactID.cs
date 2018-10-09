using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CompactID
    {
        public uint value;

        public byte N
        {
            get { return (byte) (value & 0xFF); }
        }

        public uint guidIndex
        {
            get { return value >> 8; }
        }
    }
}
