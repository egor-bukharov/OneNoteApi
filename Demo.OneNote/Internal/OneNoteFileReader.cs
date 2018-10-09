using System;
using System.Collections.Generic;
using System.IO;
using Demo.OneNote.Exceptions;

namespace Demo.OneNote.Internal
{
    public class OneNoteFileReader
    {
        public static readonly Guid FileFormatConstant = new Guid("{109ADD3F-911B-49F5-A5D0-1791EDC8AED8}");
        public const ulong FileNodeListFragmentHeaderMagic = 0xA4567AB1F5F7F4C4;

        private readonly BinaryReader binaryReader;

        public OneNoteFileReader(BinaryReader binaryReader)
        {
            this.binaryReader = binaryReader;
        }

        public void ReadHeader(ref Header header)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref header))
            {
                throw new FileFormatException("File header is truncated");
            }

            if (header.guidFileFormat != FileFormatConstant)
            {
                throw new FileFormatException("File has an invalid format");
            }

            // TODO: Validate the rest of Header members
            // ...
        }

        public void ReadTransactionEntry(ref TransactionEntry transactionEntry)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref transactionEntry))
            {
                throw new FileFormatException("Cannot read transaction entry");
            }
        }

        public void ReadFileNodeHeader(ref FileNodeHeader fileNodeHeader)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref fileNodeHeader))
            {
                throw new FileFormatException("Cannot read header of FileNode");
            }
        }

        public void ReadFileNodeListHeader(ref FileNodeListHeader fileNodeListHeader)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref fileNodeListHeader))
            {
                throw new FileFormatException("Cannot read header of FileNodeListFragment");
            }

            if (fileNodeListHeader.uintMagic != FileNodeListFragmentHeaderMagic)
            {
                throw new FileFormatException("Header of FileNodeListFragment is corrupted");
            }
        }

        public IFileChunkReference ReadFileChunkReference(StpFormat stpFormat, CbFormat cbFormat, string context)
        {
            if (stpFormat == StpFormat.Uncompressed8Bytes && cbFormat == CbFormat.Uncompressed4Bytes)
            {
                var fileChunkReference64x32 = new FileChunkReference64x32();
                ReadFileChunkReference64x32(ref fileChunkReference64x32, context);

                return fileChunkReference64x32;
            }

            if (stpFormat == StpFormat.Uncompressed4Bytes && cbFormat == CbFormat.Uncompressed4Bytes)
            {
                var fileChunkReference32 = new FileChunkReference32();
                ReadFileChunkReference32(ref fileChunkReference32, context);

                return fileChunkReference32;
            }

            if (stpFormat == StpFormat.Compressed2Bytes && cbFormat == CbFormat.Compressed1Byte)
            {
                var fileChunkReference16x8 = new FileChunkReference16x8();
                ReadFileChunkReference16x8(ref fileChunkReference16x8, context);

                return fileChunkReference16x8;
            }

            throw new NotImplementedException("Reference format is not supported");
        }

        public void ReadFileChunkReference32(ref FileChunkReference32 fileChunkReference32, string context)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref fileChunkReference32))
            {
                throw new FileFormatException("Cannot read ReadFileChunkReference32. Context: " + context);
            }
        }

        public void ReadFileChunkReference64x32(ref FileChunkReference64x32 fileChunkReference64x32, string context)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref fileChunkReference64x32))
            {
                throw new FileFormatException("Cannot read ReadFileChunkReference32. Context: " + context);
            }
        }

        public void ReadFileChunkReference16x8(ref FileChunkReference16x8 fileChunkReference16x8, string context)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref fileChunkReference16x8))
            {
                throw new FileFormatException("Cannot read ReadFileChunkReference32. Context: " + context);
            }
        }

        public void ReadExtendedGuid(ref ExtendedGuid extendedGuid, string context)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref extendedGuid))
            {
                throw new FileFormatException("Cannot read extended GUID. Context: " + context);
            }
        }

        public PropertyID ReadPropertyID()
        {
            return new PropertyID { value = binaryReader.ReadUInt32() };
        }

        public void ReadObjectSpaceObjectStreamHeader(ref ObjectSpaceObjectStreamHeader objectSpaceObjectStreamHeader)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref objectSpaceObjectStreamHeader))
            {
                throw new FileFormatException("Cannot read ObjectSpaceObjectStreamHeader");
            }
        }

        public CompactID ReadCompactID()
        {
            return new CompactID { value = binaryReader.ReadUInt32() };
        }

        public void ExpectFileNodeListFooter()
        {
            var footer = binaryReader.ReadUInt64();
            if (footer != 0x8BC215C38233BA4B)
            {
                throw new FileFormatException("Invalid value of file node list footer");
            }
        }

        public byte ReadByte()
        {
            return binaryReader.ReadByte();
        }

        public void ReadObjectDeclaration2Body(ref ObjectDeclaration2Body body)
        {
            if (!Unsafe.ReadStruct(binaryReader, ref body))
            {
                throw new FileFormatException("Cannot read ObjectDeclaration2Body");
            }
        }

        

        public void Move(long offset, string context)
        {
            var newOffset = binaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
            if (newOffset < offset)
            {
                throw new FileFormatException("Cannot set file pointer. Context: " + context);
            }
        }

        public void Skip(uint cb)
        {
            binaryReader.BaseStream.Seek(cb, SeekOrigin.Current);
        }
    }
}
