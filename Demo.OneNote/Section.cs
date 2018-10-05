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
            {
                return Open(stream);
            }
        }

        public static Section Open(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var header = new Header();
                if (!Unsafe.ReadStruct(reader, ref header))
                {
                    throw new FormatException("File header is truncated");
                }

                if (header.guidFileFormat != FileFormat)
                {
                    throw new FormatException("File has an invalid format");
                }

                // TODO: Validate the rest of Header members
                // ...

                if (!Seek(stream, header.fcrFileNodeListRoot))
                {
                    throw new FormatException("FileNodeListRoot was not found");
                }

                while (true)
                {
                    var fileNodeListFragmentHeader = new FileNodeListFragmentHeader();
                    // TODO: This should be well-tested
                    if (!Unsafe.ReadStruct(reader, ref fileNodeListFragmentHeader))
                    {
                        throw new FormatException("Cannot read header of FileNodeListFragment");
                    }

                    // TODO: This should be well-tested
                    // TODO: Convert magic number to a constant
                    if (fileNodeListFragmentHeader.uintMagic != 0xA4567AB1F5F7F4C4)
                    {
                        throw new FormatException("FileNodeListFragment header is corrupted");
                    }

                    var fileNodeHeader = new FileNodeHeader();
                    // TODO: This should be well-tested
                    if (!Unsafe.ReadStruct(reader, ref fileNodeHeader))
                    {
                        throw new FormatException("Cannot read header of FileNode");
                    }

                    //if (fileNodeListFragmentHeader.FileNodeListID == FileNodeIDs.ObjectSpaceManifestListReferenceFND)
                    //{

                    //}
                    //else
                    //{
                            
                    //}

                    break;
                }

                return new Section();
            }
        }

        private static bool Seek(Stream stream, FileChunkReference64x32 fileChunkReference64x32)
        {
            var offset = (long) fileChunkReference64x32.Offset;
            var actualOffset = stream.Seek(offset, SeekOrigin.Begin);

            return actualOffset == offset;
        }
    }
}
