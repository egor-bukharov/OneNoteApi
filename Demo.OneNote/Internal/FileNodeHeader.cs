using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FileNodeHeader
    {
        private uint value;

        private const byte SizeBitOffset = 10;
        private const byte StpFormatBitOffset = 23;
        private const byte CbFormatBitOffset = 25;

        private const uint FileNodeIDBitMask = 0x3FF;
        private const uint SizeBitMask = 0x1FF >> SizeBitOffset;
        private const uint StpFormatBitMask = 0x3 >> StpFormatBitOffset;
        private const uint CbFormatBitMask = 0x3 >> CbFormatBitOffset;

        // TODO: This code should be well-tested
        public uint FileNodeID
        {
            get { return value & FileNodeIDBitMask; }
            set
            {
                this.value = (value & FileNodeIDBitMask) | (this.value & ~FileNodeIDBitMask);
            }
        }

        // TODO: This code should be well-tested
        public uint Size
        {
            get { return (value & SizeBitMask) << SizeBitOffset; }
            // TODO: See if this code can be optimized
            set { this.value = ((value >> SizeBitOffset) & SizeBitMask) | (this.value & ~SizeBitMask); }
        }

        // TODO: This code should be well-tested
        public StpFormat StpFormat
        {
            get
            {
                return (StpFormat)((value & StpFormatBitMask) << StpFormatBitOffset);
            }
            set { this.value = (((uint) value >> StpFormatBitOffset) & StpFormatBitMask) | (this.value & ~StpFormatBitMask); }
        }

        // TODO: This code should be well-tested
        public CbFormat CbFormat
        {
            get
            {
                return (CbFormat)((value & CbFormatBitMask) << CbFormatBitOffset);
            }
            set { this.value = (((uint)value >> CbFormatBitOffset) & CbFormatBitMask) | (this.value & ~CbFormatBitMask); }
        }
    }
}
