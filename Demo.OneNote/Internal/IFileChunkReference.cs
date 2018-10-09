namespace Demo.OneNote.Internal
{
    public interface IFileChunkReference
    {
        long Offset { get; }
        uint DataSize { get; }
        uint SizeOfReferenceStructure { get; }
    }
}
