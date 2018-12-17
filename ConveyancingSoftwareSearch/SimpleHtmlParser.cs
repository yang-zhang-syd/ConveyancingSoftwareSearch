using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConveyancingSoftwareSearch
{
    public interface IHtmlParser
    {
        List<int> GetMatchIndices(Regex regex);
        string ReadDivAtIndex(int index);
    }

    public class SimpleHtmlParser : IHtmlParser
    {
        private readonly string _html;

        public SimpleHtmlParser(string html)
        {
            _html = html;
        }

        public List<int> GetMatchIndices(Regex regex)
        {
            var matches = regex.Matches(_html);
            var indices = matches
                .Select(m => m.Index)
                .OrderBy(i => i)
                .ToList();

            return indices;
        }

        public string ReadDivAtIndex(int index)
        {
            var sr = new StringReader(_html.Substring(index));

            var toReturn = "";
            var readResult = sr.ReadUntilNextOpenBracket();
            if (!readResult.StartsWith("<div"))
            {
                // expect first open div
                throw new Exception("Expecting start with '<div'");
            }

            toReturn += readResult;

            var stack = new Stack<string>();
            stack.Push("<div");

            while (stack.Count != 0)
            {
                readResult = sr.ReadUntilNextOpenBracket();
                if (readResult.StartsWith("<div"))
                {
                    // open div found
                    stack.Push("<div");
                }
                else if (readResult.StartsWith("</div"))
                {
                    // close div found
                    stack.Pop();
                }

                toReturn += readResult;
            }

            return toReturn;
        }
    }
}