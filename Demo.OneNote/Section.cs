using System;
using System.IO;
using Demo.OneNote.Internal;

namespace Demo.OneNote
{
    public class Section
    {
        protected internal static readonly Guid FileFormat = new Guid("{109ADD3F-911B-49F5-A5D0-1791EDC8AED8}");

        private Section() { }

        public static Section Open(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new BinaryReader(stream))
            {
                var header = new Header();
                if (!Interop.ReadStruct(reader, ref header))
                {
                    throw new FormatException("File header is truncated");
                }

                if (header.FileFormat != FileFormat)
                {
                    throw new FormatException("File has an invalid format");
                }

                // TODO: Below code should be either well-tested or removed
                //if (!Validator.ValidateFileFormat(header.LastCodeThatWroteToThisFile))
                //{
                //    throw new FormatException("Field LastCodeThatWroteToThisFile has invalid value");
                //}

                // TODO: Validation for all members of Header instance
                // ...

                return new Section();
            }
        }

        
    }
}
