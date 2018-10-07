using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TransactionEntry
    {
        public const uint SentinelEntryId = 0x00000001;

        public static readonly uint SizeInBytes = (uint) Marshal.SizeOf(typeof(TransactionEntry));

        public uint srcID;
        public uint TransactionEntrySwitch;
    }
}
