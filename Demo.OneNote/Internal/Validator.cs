namespace Demo.OneNote.Internal
{
    internal static class Validator
    {
        public static bool ValidateFileFormat(FileFormat value)
        {
            return value == FileFormat.One || value == FileFormat.Onetoc2;
        }
    }
}
