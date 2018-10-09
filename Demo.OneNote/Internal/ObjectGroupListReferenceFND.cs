namespace Demo.OneNote.Internal
{
    public class ObjectGroupListReferenceFND : FNDBase
    {
        public readonly IFileChunkReference fileChunkReference;
        public readonly ExtendedGuid ObjectGroupID;

        public ObjectGroupListReferenceFND(FileNodeHeader header, IFileChunkReference fileChunkReference, ExtendedGuid objectGroupId) : base(header)
        {
            this.fileChunkReference = fileChunkReference;
            ObjectGroupID = objectGroupId;
        }
    }
}
