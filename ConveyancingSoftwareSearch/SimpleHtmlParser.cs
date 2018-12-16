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
        private List<string> supportedTags = new List<string>
        {
            "html",
            "body",
            "main",
            "div"
        };

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

            var tag = parseTag(streamReader, ((char)firstChar).ToString());

            return tag.Result;
        }

        /// <summary>
        /// Parse a html tag.
        /// </summary>
        /// <param name="sr">Stream reader</param>
        /// <param name="first">first char of the tag</param>
        /// <returns>Tag element and the next non whitespace char.</returns>
        private ResultAndReadText<HtmlTag> parseTag(StreamReader sr, string readText = null)
        {
            if (readText == null)
            {
                readText = sr.ReadNextNonWhiteSpace().ToString();
            }

            if (!readText.StartsWith("<"))
            {
                throw new InvalidOperationException("Cannot parse a tag not starting with '<'");
            }

            var next = readText.Length >= 2 ? readText[1] : (char)sr.Peek();
            if (next == '!')
            {
                // If this is a comment, continue to parse next tag.
                readText = skipComment(sr);
                if (readText.StartsWith('<'))
                {
                    return parseTag(sr, readText);
                }

                return null;
            }

            var tagNameResult = readTagName(sr, readText);
            var tagName = tagNameResult.Result;
            readText = tagNameResult.ReadText;

            // read attributes
            var attributeResult = readAttributes(sr, readText);
            readText = attributeResult.ReadText;

            var htmlTag = new HtmlTag();
            htmlTag.TagName = tagName;
            htmlTag.Attributes = attributeResult.Result;

            if (readText.StartsWith("/"))
            {
                // self closed tag
                var nextRead = sr.ReadChar(); // this will be '>' otherwise incorrect
                if (nextRead != '>')
                {
                    throw new Exception("No '>' found after '/'.");
                }

                nextRead = sr.ReadNextNonWhiteSpace();
                htmlTag.IsSelfClosed = true;
                return new ResultAndReadText<HtmlTag>(htmlTag, nextRead.ToString());
            }

            if (readText.StartsWith('>'))
            {
                // find nesting tag
                readText = sr.ReadNextNonWhiteSpace().ToString();
            }

            var tagsStack = new Stack<string>();
            tagsStack.Push(tagName);

            while (true)
            {
                if (readText.StartsWith('<'))
                {
                    var secondNext = sr.Peek();
                    if (secondNext == '/')
                    {
                        // this is to close the html tag
                        var closeTagResult = readClosingTag(sr);
                        var closedTagName = closeTagResult.Result;
                        readText = closeTagResult.ReadText;

                        if (tagsStack.Peek() != closedTagName)
                        {
                            throw new Exception("Closed tag does not match top of tags stack!");
                        }

                        tagsStack.Pop();

                        if (tagsStack.Count == 0)
                        {
                            break;
                        }

                        continue;
                    }

                    var nextWord = sr.ReadWord();
                    readText += nextWord;
                    if (supportedTags.Contains(nextWord.ToLower()))
                    {
                        // find a child html tag here
                        var child = parseTag(sr, readText);
                        htmlTag.Children.Add(child.Result);
                        readText = child.ReadText;
                        if (!child.Result.IsSelfClosed)
                        {
                            tagsStack.Push(child.Result.TagName);
                        }
                    }
                    else
                    {
                        // find an unsupported tag
                        var child = parseUnsupportedTag(sr, readText, nextWord);
                        htmlTag.Children.Add(child.Result);
                        readText = child.ReadText;
                    }
                }
                else
                {
                    // find inner text here
                    var innerTextResult = readInnerText(sr, readText, tagName);
                    var textElement = innerTextResult.Result;
                    htmlTag.Children.Add(textElement);
                    readText = innerTextResult.ReadText;
                }
            }
            
            return new ResultAndReadText<HtmlTag>(htmlTag, readText);
        }

        private ResultAndReadText<UnsupportedHtmlTag> parseUnsupportedTag(StreamReader sr, string readText, string tagName)
        {
            var closingPattern = $"</{tagName}>";
            var sb = new StringBuilder(readText);
            var finished = false;

            while (!finished)
            {
                var c = (char)sr.Read();
                if (c == '<')
                {
                    var closingTagSb = new StringBuilder();
                    closingTagSb.Append(c);
                    
                    while (true)
                    {
                        c = sr.ReadChar();
                        closingTagSb.Append(c);
                        var closeTagRead = closingTagSb.ToString();
                        if (!closingPattern.StartsWith(closeTagRead))
                        {
                            sb.Append(closeTagRead);
                            break;
                        }

                        if (closeTagRead == closingPattern)
                        {
                            sb.Append(closeTagRead);
                            finished = true;
                            break;
                        }
                    }
                }
                else if (c == '/' && sr.Peek() == '>')
                {
                    sb.Append(c);
                    c = (char)sr.Read();
                    sb.Append(c);
                    finished = true;
                }
                else
                {
                    sb.Append(c);
                }
            }

            var tag = sb.ToString();
            var nextCharString = sr.ReadNextNonWhiteSpace().ToString();
            return new ResultAndReadText<UnsupportedHtmlTag>(new UnsupportedHtmlTag(tagName, tag), nextCharString);
        }

        private string skipComment(StreamReader sr)
        {
            int c;
            while ((c = sr.ReadNextNonWhiteSpace()) != '>')
            {
                // Just ignore the comment tag for now.
            }

            var next = sr.ReadNextNonWhiteSpace();
            return next.ToString();
        }

        private ResultAndReadText<string> readClosingTag(StreamReader sr)
        {
            var c = sr.ReadChar(); // '/'
            var tagName = sr.ReadWord();
            c = sr.ReadChar(); // '>'
            c = sr.ReadNextNonWhiteSpace(); // next to process
            return new ResultAndReadText<string>(tagName, c.ToString());
        }

        private ResultAndReadText<string> readTagName(StreamReader sr, string readText)
        {
            readText = readText.Replace("<", "");
            int idx = readText.IndexOf(' ');
            if (idx != -1)
            {
                return new ResultAndReadText<string>(readText.Substring(0, idx + 1), readText.Substring(idx + 1));
            }
            
            var newText = sr.ReadWord();
            var nextCharString = sr.ReadNextNonWhiteSpace().ToString();
            return new ResultAndReadText<string> (readText + newText, nextCharString);
        }

        private ResultAndReadText<HtmlAttribute> readAttributes(StreamReader sr, string readText)
        {
            int idx = readText.IndexOf('>');
            if (idx != -1)
            {
                var attributeStr = readText.Substring(0, idx);
                readText = readText.Substring(idx + 1);
                if (string.IsNullOrEmpty(readText))
                {
                    readText = ((char)sr.Read()).ToString();
                }
                return new ResultAndReadText<HtmlAttribute>(new HtmlAttribute(attributeStr), readText);
            }

            char c;
            var sb = new StringBuilder(readText);
            var isInDoubleQuote = false;

            while ((c = (char)sr.Peek()) != '>' && c != '/' || isInDoubleQuote)
            {
                c = (char) sr.Read();
                isInDoubleQuote = c == '"' ? !isInDoubleQuote : isInDoubleQuote;
                sb.Append(c);
            }

            var nextCharString = sr.ReadNextNonWhiteSpace().ToString();
            return new ResultAndReadText<HtmlAttribute> (new HtmlAttribute(sb.ToString()), nextCharString);
        }

        private ResultAndReadText<InnerText> readInnerText(StreamReader sr, string readText, string tagName)
        {
            char c;
            var sb = new StringBuilder(readText);

            // TODO: this could not handle where text includes '<' as contents.
            while ((c = sr.ReadChar()) != '<')
            {
                sb.Append((char)c);
            }
            
            return new ResultAndReadText<InnerText>(new InnerText(sb.ToString()), c.ToString());
        }
    }
}
