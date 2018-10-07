using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileNodeHeader
    {
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
            set
            {
                this.Value = (value & FileNodeIDBitMask) | (this.Value & ~FileNodeIDBitMask);
            }
        }

        public uint Size
        {
            get { return (Value & SizeBitMask) >> SizeBitOffset; }
            // TODO: See if this code can be optimized
            set { this.Value = ((value << SizeBitOffset) & SizeBitMask) | (this.Value & ~SizeBitMask); }
        }

        public StpFormat StpFormat
        {
            get
            {
                return (StpFormat)((Value & StpFormatBitMask) >> StpFormatBitOffset);
            }
            set { this.Value = (((uint) value << StpFormatBitOffset) & StpFormatBitMask) | (this.Value & ~StpFormatBitMask); }
        }

        public CbFormat CbFormat
        {
            get
            {
                return (CbFormat)((Value & CbFormatBitMask) >> CbFormatBitOffset);
            }
            set { this.Value = (((uint)value << CbFormatBitOffset) & CbFormatBitMask) | (this.Value & ~CbFormatBitMask); }
        }
    }
}
