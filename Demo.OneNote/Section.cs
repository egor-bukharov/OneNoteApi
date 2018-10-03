using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Demo.OneNote
{
    public class Section
    {
        protected Section() { }

        public static Section Open(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return new Section();
            }
        }
    }
}
