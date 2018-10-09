using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PropertyID
    {
        public uint value;

        public uint Id
        {
            get { return value & 0x3FFFFFF; }
        }

        public byte Type
        {
            get { return (byte) ((value >> 26) & 0x1F); }
        }
    }
}
