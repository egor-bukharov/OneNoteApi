namespace Demo.OneNote.Internal
{
    public class ObjectSpaceObjectPropSet
    {
        public readonly ObjectSpaceObjectStream OIDs;
        public readonly ObjectSpaceObjectStream OSIDs;
        public readonly ObjectSpaceObjectStream ContextIDs;

        public ObjectSpaceObjectPropSet(ObjectSpaceObjectStream oids, ObjectSpaceObjectStream osids, ObjectSpaceObjectStream contextIDs)
        {
            OIDs = oids;
            OSIDs = osids;
            ContextIDs = contextIDs;
        }
    }
}
