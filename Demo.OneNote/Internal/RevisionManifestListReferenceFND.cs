namespace Demo.OneNote.Internal
{
    public class RevisionManifestListReferenceFND : FNDBase
    {
        public readonly IFileChunkReference fileChunkReference;

        public RevisionManifestListReferenceFND(FileNodeHeader header, IFileChunkReference fileChunkReference) : base(header)
        {
            this.fileChunkReference = fileChunkReference;
        }
    }
}
