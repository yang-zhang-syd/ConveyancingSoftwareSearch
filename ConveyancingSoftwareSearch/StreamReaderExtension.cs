using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public static class StreamReaderExtension
    {
        public static int ReadNextNonWhiteSpace(this StreamReader sr)
        {
            var c = (char)sr.Read();
            while (char.IsWhiteSpace(c))
            {
                c = (char) sr.Read();
            }

            return c;
        }
    }
}
