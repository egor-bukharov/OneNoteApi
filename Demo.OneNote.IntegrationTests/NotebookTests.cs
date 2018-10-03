using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.OneNote.IntegrationTests
{
    [TestClass]
    public class NotebookTests
    {
        [TestMethod]
        public void OpenedNotebookShouldNotBeNull()
        {
            const string relativePathToInputFile = @"TestInputFiles\OneNoteApi Demo.one";
            var notebook = Notebook.Open(relativePathToInputFile);

            Assert.IsNotNull(notebook);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowAnExceptionIfFIleNotExists()
        {
            const string relativePathToInputFile = @"TestInputFiles\Not existing file.one";
            Notebook.Open(relativePathToInputFile);
        }
    }
}
