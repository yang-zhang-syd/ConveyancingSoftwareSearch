using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConveyancingSoftwareSearch
{
    public class SimpleHtmlParser
    {
        /// <summary>
        /// Parse a stream from http response.
        /// </summary>
        /// <param name="responseStream">Http response stream</param>
        /// <returns>Root Html Tag</returns>
        public HtmlTag ParseHtml(Stream responseStream)
        {
            var streamReader = new StreamReader(responseStream);
            var firstChar = streamReader.ReadNextNonWhiteSpace();

            if (firstChar == -1)
            {
                return null;
            }

            if (firstChar == '<')
            {
                var result = readTagName(streamReader);
                var tagName = result.Result;
                if (tagName.StartsWith('!'))
                {
                    // This is a comment. Read to the end and return next char after the comment.
                    firstChar = readComment(streamReader);
                }
            }

            return null;
        }

        private ResultAndNextRead<HtmlTag> parseTag(StreamReader sr, int? first = null)
        {
            if (first != null || first.Value != '<')
            {
                throw new InvalidOperationException("Cannot parse a tag not starting with '<'");
            }

            var result = readTagName(sr);
            var tagName = result.Result;
            var nextRead = result.NextRead;

            if (tagName.StartsWith('!'))
            {
                // If this is a comment, continue to parse next tag.
                var next = readComment(sr);
                if (next == '<')
                {
                    return parseTag(sr, next);
                }
                else
                {
                    return null;
                }
            }

            var htmlTag = new HtmlTag();
            htmlTag.TagName = tagName;

            if (nextRead == '/')
            {
                // self closed tag
                nextRead = sr.Read(); // this will be '>' otherwise incorrect
                if (nextRead != '>')
                {
                    throw new Exception("No '>' found after '/'.");
                }

                nextRead = sr.ReadNextNonWhiteSpace();
                return new ResultAndNextRead<HtmlTag>(htmlTag, nextRead);
            }
            else if(nextRead == '>')
            {
                var next = sr.ReadNextNonWhiteSpace();
                if (next == '<')
                {
                    // find a child html tag here
                    var child = parseTag(sr, next);
                    htmlTag.Children.Add(child.Result);
                    nextRead = child.NextRead;
                }
                else
                {
                    // find inner text here
                    var innerTextResult = readInnerText(sr, next);
                    var text = innerTextResult.Result;
                    htmlTag.Children.Add(text);
                    nextRead = innerTextResult.NextRead;
                }
            }
            else
            {
                // read attributes
                var attributeResult = readAttribute(sr);
                htmlTag.Attribute = attributeResult.Result;
            }

            return null;
        }

        private int readComment(StreamReader sr)
        {
            int c;
            while ((c = sr.ReadNextNonWhiteSpace()) != '>')
            {
                // Just ignore the comment tag for now.
            }

            var next = sr.ReadNextNonWhiteSpace();
            return next;
        }

        private ResultAndNextRead<string> readTagName(StreamReader sr)
        {
            int c;
            var sb = new StringBuilder();

            while ((c = sr.Read()) != ' ' && c != '>' && c != '/')
            {
                sb.Append(c);
            }

            return new ResultAndNextRead<string> (sb.ToString(), c);
        }

        private ResultAndNextRead<HtmlAttribute> readAttribute(StreamReader sr)
        {
            int c;
            var sb = new StringBuilder();

            while ((c = sr.Read()) != '>' && c != '/')
            {
                sb.Append(c);
            }

            return new ResultAndNextRead<HtmlAttribute> (new HtmlAttribute(sb.ToString()), c);
        }

        private ResultAndNextRead<InnerText> readInnerText(StreamReader sr, int next)
        {
            int c;
            var sb = new StringBuilder();
            sb.Append(next);

            while ((c = sr.Read()) != '>' && c != '/' && c != '<')
            {
                sb.Append(c);
            }

            return new ResultAndNextRead<InnerText>(new InnerText(sb.ToString()), c);
        }
    }
}
