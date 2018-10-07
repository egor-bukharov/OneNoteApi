using System;

namespace Demo.OneNote.Exceptions
{
    public class FileFormatException : Exception
    {
        public FileFormatException(string message) : base(message)
        {
        }
    }
}
