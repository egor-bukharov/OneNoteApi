using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Demo.OneNote
{
    public class Notebook
    {
        protected Notebook() { }

        public static Notebook Open(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return new Notebook();
            }
        }
    }
}
