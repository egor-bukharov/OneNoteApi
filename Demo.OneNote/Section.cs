﻿using System;
using System.IO;
using Demo.OneNote.Internal;

namespace Demo.OneNote
{
    public class Section
    {
        protected internal static readonly Guid FileFormat = new Guid("{109ADD3F-911B-49F5-A5D0-1791EDC8AED8}");

        public Section() { }

        public static Section Open(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new BinaryReader(stream))
            {
                var fileReader = new OneNoteFileReader(reader);
                var sectionReader = new SectionReader(fileReader);
                return sectionReader.ReadSection();
            }
        }
    }
}
