using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Demo.OneNote.Exceptions;

namespace Demo.OneNote.Internal
{
    public class SectionReader
    {
        public static readonly Guid FileFormatConstant = new Guid("{109ADD3F-911B-49F5-A5D0-1791EDC8AED8}");
        public const ulong FileNodeListFragmentHeaderMagic = 0xA4567AB1F5F7F4C4;

        private readonly BinaryReader reader;

        public SectionReader(BinaryReader reader)
        {
            this.reader = reader;
        }

        public void ReadHeader(ref Header header)
        {
            if (!Unsafe.ReadStruct(reader, ref header))
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

        public bool ReadFileNodeListHeader(FileChunkReference64x32 fileChunkReference64x32, ref FileNodeListHeader fileNodeListHeader)
        {
            Move(fileChunkReference64x32.stp);

            if (!Unsafe.ReadStruct(reader, ref fileNodeListHeader))
            {
                throw new FileFormatException("Cannot read header of FileNodeListFragment");
            }

            if (fileNodeListHeader.uintMagic != FileNodeListFragmentHeaderMagic)
            {
                throw new FileFormatException("Header of FileNodeListFragment is corrupted");
            }

            return true;
        }

        public bool ReadFileNodeHeader(ref FileNodeHeader fileNodeHeader)
        {
            if (!Unsafe.ReadStruct(reader, ref fileNodeHeader))
            {
                throw new FileFormatException("Cannot read header of FileNode");
            }

            // TODO: Validate fileNodeHeader members

            return true;
        }

        public void ReadTransactionLog(Dictionary<uint, uint> transactionsData, uint cTransactionsInLog, uint cbTransactionLogFragmentSize)
        {
            uint cbRead = 0;
            var cbMax = cbTransactionLogFragmentSize - FileChunkReference64x32.SizeInBytes;

            for (var i = 0; i < cTransactionsInLog; i++)
            {
                if (cbRead == cbMax)
                {
                    var nextFragment = new FileChunkReference64x32();
                    ReadFileChunkReference64X32(ref nextFragment);

                    Move(nextFragment.stp);

                    cbRead = 0;
                    cbMax = nextFragment.cb - FileChunkReference64x32.SizeInBytes;
                }
                else if (cbRead > cbMax)
                {
                    throw new FileFormatException("Transaction log is broken");
                }

                ReadTransactionFromLog(transactionsData, ref cbRead);
            }
        }

        public void ReadTransactionFromLog(Dictionary<uint, uint> transactionsData, ref uint cb)
        {
            var transactionEntry = new TransactionEntry();
            while (true)
            {
                ReadTransactionEntry(ref transactionEntry);
                cb += TransactionEntry.SizeInBytes;

                if (transactionEntry.srcID == TransactionEntry.SentinelEntryId)
                {
                    break;
                }

                transactionsData[transactionEntry.srcID] = transactionEntry.TransactionEntrySwitch;
            }
        }

        public void ReadTransactionEntry(ref TransactionEntry transactionEntry)
        {
            if (!Unsafe.ReadStruct(reader, ref transactionEntry))
            {
                throw new FileFormatException("Cannot read transaction entry");
            }
        }

        public void ReadFileChunkReference64X32(ref FileChunkReference64x32 fileChunkReference64x32)
        {
            if (!Unsafe.ReadStruct(reader, ref fileChunkReference64x32))
            {
                throw new FileFormatException("Cannot read reference to the next transaction log fragment");
            }
        }

        public void Move(ulong offset)
        {
            reader.BaseStream.Seek((long) offset, SeekOrigin.Begin);
        }

        public void Skip(uint cb)
        {
            reader.BaseStream.Seek(cb, SeekOrigin.Current);
        }
    }
}
