namespace Demo.OneNote.Internal
{
    public class ObjectSpaceManifestListReferenceFND : FNDBase
    {
        public readonly IFileChunkReference fileChunkReference;
        public readonly ExtendedGuid gosid;

        public ObjectSpaceManifestListReferenceFND(FileNodeHeader header, IFileChunkReference fileChunkReference, ExtendedGuid gosid)
            : base(header)
        {
            this.fileChunkReference = fileChunkReference;
            this.gosid = gosid;
        }
    }
}
