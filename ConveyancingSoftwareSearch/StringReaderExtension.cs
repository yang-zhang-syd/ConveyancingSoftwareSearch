using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public static class StringReaderExtension
    {
        public static string SkipNonLetterChars(this StringReader sr)
        {
            var sb = new StringBuilder();
            while (!char.IsLetter((char)sr.Peek()))
            {
                var c = (char)sr.Read();
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static string ReadUntilNextOpenBracket(this StringReader sr)
        {
            var first = (char)sr.Read();
            var sb = new StringBuilder(first.ToString());
            while (((char) sr.Peek()) != '<')
            {
                var c = (char)sr.Read();
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
