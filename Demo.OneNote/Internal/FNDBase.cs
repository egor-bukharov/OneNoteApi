namespace Demo.OneNote.Internal
{
    public abstract class FNDBase
    {
        public readonly FileNodeHeader header;

        protected FNDBase(FileNodeHeader header)
        {
            this.header = header;
        }
    }
}
