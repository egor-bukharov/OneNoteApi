using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileChunkReference64x32
    {
        public static readonly uint SizeInBytes = (uint)Marshal.SizeOf(typeof(FileChunkReference64x32));

        public const ulong OffsetValueOfNil = 0xFFFFFFFFFFFFFFFF;
        
        public ulong stp;
        public uint cb;

        public override string ToString()
        {
            if (cb == 0)
            {
                if (stp == 0)
                {
                    return "Zero";
                }

                if (stp == OffsetValueOfNil)
                {
                    return "Nil";
                }
            }

            return string.Format("stp: {0}, cb: {1}", stp, cb);
        }

        public static FileChunkReference64x32 Zero
        {
            get { return new FileChunkReference64x32(); }
        }

        public static FileChunkReference64x32 Nil
        {
            get { return new FileChunkReference64x32 { stp = OffsetValueOfNil, cb = 0 }; }
        }
    }
}
