using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileNodeHeader
    {
        public static readonly uint SizeInBytes = (uint)Marshal.SizeOf(typeof(FileNodeHeader));

        public uint Value;

        private const byte SizeBitOffset = 10;
        private const byte StpFormatBitOffset = 23;
        private const byte CbFormatBitOffset = 25;

        private const uint FileNodeIDBitMask = 0x3FF;
        private const uint SizeBitMask = 0x1FFF << SizeBitOffset;
        private const uint StpFormatBitMask = 0x3 << StpFormatBitOffset;
        private const uint CbFormatBitMask = 0x3 << CbFormatBitOffset;

        public uint FileNodeID
        {
            get { return Value & FileNodeIDBitMask; }
        }

        public uint Size
        {
            get { return (Value & SizeBitMask) >> SizeBitOffset; }
        }

        public StpFormat StpFormat
        {
            get
            {
                return (StpFormat)((Value & StpFormatBitMask) >> StpFormatBitOffset);
            }
        }

        public CbFormat CbFormat
        {
            get
            {
                return (CbFormat)((Value & CbFormatBitMask) >> CbFormatBitOffset);
            }
        }
    }
}
