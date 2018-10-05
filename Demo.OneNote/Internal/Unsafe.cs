using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    internal static class Unsafe
    {
        public static bool ReadStruct<T>(BinaryReader reader, ref T result)
        {
            var expectedLength = Marshal.SizeOf(typeof(T));
            var bytes = reader.ReadBytes(expectedLength);

            if (bytes.Length < expectedLength)
            {
                return false;
            }

            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return true;
        }
    }
}
