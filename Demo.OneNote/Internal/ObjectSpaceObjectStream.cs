using System.Collections.Generic;

namespace Demo.OneNote.Internal
{
    public class ObjectSpaceObjectStream
    {
        public readonly ObjectSpaceObjectStreamHeader Header;
        public readonly ICollection<CompactID> Items;

        public ObjectSpaceObjectStream(ObjectSpaceObjectStreamHeader header, ICollection<CompactID> items)
        {
            Header = header;
            Items = items;
        }
    }
}
