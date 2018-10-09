using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileNodeListHeader
    {
        public static readonly uint SizeInBytes = (uint) Marshal.SizeOf(typeof(FileNodeListHeader));

        public ulong uintMagic; // must be "0xA4567AB1F5F7F4C4"
        public uint FileNodeListID; //  must be equal to or greater than 0x00000010
        public uint nFragmentSequence; 
    }
}
