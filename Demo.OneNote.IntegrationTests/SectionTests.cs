﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.IntegrationTests
{
    [TestClass]
    public class SectionTests
    {
        [TestMethod]
        public void OpenedSectionShouldNotBeNull()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";
            var notebook = Section.Open(relativePathToInputFile);

            Assert.IsNotNull(notebook);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowAnExceptionIfFIleNotExists()
        {
            const string relativePathToInputFile = @"TestInputFiles\Not existing file.one";
            Section.Open(relativePathToInputFile);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ShouldThrowAnExceptionIfFileHasInvalidFormat()
        {
            const string relativePathToInputFile = @"TestInputFiles\InvalidGuidFileFormat.one";
            Section.Open(relativePathToInputFile);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ShouldThrowAnExceptionIfFileSizeIsLessThanHeaderSize()
        {
            const string relativePathToInputFile = @"TestInputFiles\TruncatedHeader.one";
            Section.Open(relativePathToInputFile);
        }
    }
}
