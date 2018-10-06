using System;
using System.IO;

namespace Demo.OneNote.Internal
{
    public class SectionReader
    {
        public static readonly Guid FileFormatConstant = new Guid("{109ADD3F-911B-49F5-A5D0-1791EDC8AED8}");

        public static readonly Guid FileTypeOne = new Guid("{7B5C52E4-D88C-4DA7-AEB1-5378D02996D3}");
        public static readonly Guid FileTypeOnetoc2 = new Guid("{43FF2FA1-EFD9-4C76-9EE2-10EA5722765F}");

        public void ReadHeader(BinaryReader reader, ref Header header)
        {
            if (!Unsafe.ReadStruct(reader, ref header))
            {
                throw new FormatException("File header is truncated");
            }

            if (header.guidFileFormat != FileFormatConstant)
            {
                throw new FormatException("File has an invalid format");
            }

            // TODO: Validate the rest of Header members
            // ...
        }

        //public static Section Open(BinaryReader reader)
        //{
        //    var header = new Header();
            

        //    if (!Seek(stream, header.fcrFileNodeListRoot))
        //    {
        //        throw new FormatException("FileNodeListRoot was not found");
        //    }

        //    while (true)
        //    {
        //        var fileNodeListFragmentHeader = new FileNodeListFragmentHeader();
        //        // TODO: This should be well-tested
        //        if (!Unsafe.ReadStruct(reader, ref fileNodeListFragmentHeader))
        //        {
        //            throw new FormatException("Cannot read header of FileNodeListFragment");
        //        }

        //        // TODO: This should be well-tested
        //        // TODO: Convert magic number to a constant
        //        if (fileNodeListFragmentHeader.uintMagic != 0xA4567AB1F5F7F4C4)
        //        {
        //            throw new FormatException("FileNodeListFragment header is corrupted");
        //        }

        //        var fileNodeHeader = new FileNodeHeader();
        //        // TODO: This should be well-tested
        //        if (!Unsafe.ReadStruct(reader, ref fileNodeHeader))
        //        {
        //            throw new FormatException("Cannot read header of FileNode");
        //        }



        //        //if (fileNodeListFragmentHeader.FileNodeListID == FileNodeIDs.ObjectSpaceManifestListReferenceFND)
        //        //{

        //        //}
        //        //else
        //        //{

        //        //}

        //        stream.Seek(fileNodeHeader.Size, SeekOrigin.Current);


        //        break;
        //    }
        //}

        //private static bool Seek(Stream stream, FileChunkReference64x32 fileChunkReference64x32)
        //{
        //    var offset = (long)fileChunkReference64x32.Offset;
        //    var actualOffset = stream.Seek(offset, SeekOrigin.Begin);

        //    return actualOffset == offset;
        //}
    }
}
