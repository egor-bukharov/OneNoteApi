using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ObjectSpaceObjectStreamHeader
    {
        public uint value;

        public uint Count
        {
            get { return value & 0xFFFFFF; }
        }

        public bool ExtendedStreamsPresent
        {
            get { return (value & 0x40000000) != 0; }
        }

        public bool OsidStreamNotPresent
        {
            get { return (value & 0x80000000) != 0; }
        }
    }
}
