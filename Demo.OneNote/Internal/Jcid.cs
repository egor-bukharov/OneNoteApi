using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Jcid
    {
        public uint value;

        public bool PropertySet
        {
            get { return (value & 0x20000) != 0; }
        }
    }
}
