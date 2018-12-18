using System.IO;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public static class StringReaderExtension
    {
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
