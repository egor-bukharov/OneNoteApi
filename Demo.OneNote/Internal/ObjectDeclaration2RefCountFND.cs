namespace Demo.OneNote.Internal
{
    public class ObjectDeclaration2RefCountFND : FNDBase
    {
        public readonly IFileChunkReference blobFileChunkReference;
        public readonly ObjectDeclaration2Body body;
        public readonly byte cRef;

        public ObjectDeclaration2RefCountFND(FileNodeHeader header, IFileChunkReference blobFileChunkReference, ObjectDeclaration2Body body, byte cRef) : base(header)
        {
            this.blobFileChunkReference = blobFileChunkReference;
            this.body = body;
            this.cRef = cRef;
        }
    }
}
