using System;
using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ExtendedGuid
    {
        public Guid Guid;
        public uint IsZero;

        public static uint SizeInBytes = (uint) Marshal.SizeOf(typeof(ExtendedGuid));
    }
}
