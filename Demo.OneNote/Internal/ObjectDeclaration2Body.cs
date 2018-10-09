using System.Runtime.InteropServices;

namespace Demo.OneNote.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ObjectDeclaration2Body
    {
        public static readonly uint SizeInBytes = (uint) Marshal.SizeOf(typeof(ObjectDeclaration2Body));

        public uint oid;
        public Jcid jcid;
        public byte flags;
    }
}
