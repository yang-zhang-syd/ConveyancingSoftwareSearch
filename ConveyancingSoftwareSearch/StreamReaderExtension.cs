using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public static class StreamReaderExtension
    {
        public static char ReadNextNonWhiteSpace(this StreamReader sr)
        {
            var c = (char)sr.Read();
            while (char.IsWhiteSpace(c))
            {
                c = (char) sr.Read();
            }

            return c;
        }

        public static char ReadChar(this StreamReader sr)
        {
            return (char)sr.Read();
        }

        public static string ReadWord(this StreamReader sr)
        {
            var sb = new StringBuilder();
            while (char.IsLetter((char)sr.Peek()))
            {
                sb.Append((char) sr.Read());
            }

            return sb.ToString();
        }
    }
}
