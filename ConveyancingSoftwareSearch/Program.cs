using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace ConveyancingSoftwareSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var keywordString = "conveyancing software";
            var numOfResults = 100;

            var url = GoogleSearchUrlBuilder.Build(keywordString, numOfResults);
            
            IWebRequestSender webRequestSender = new WebRequestSender();
            var html = webRequestSender.Get(url);

            var parser = new SimpleHtmlParser(html);

            var matchIndices = parser.GetMatchIndices(new Regex("<div class=\"g\">"));
            var resultIndicesContainingSmokeBallUrl = new List<int>();

            for (int i = 0; i < matchIndices.Count; ++i)
            {
                var div = parser.ReadDivAtIndex(matchIndices[i]);

                var smokeBallUrlRegex = new Regex(@"www\.smokeball\.com\.au");
                var urlMatches = smokeBallUrlRegex.Matches(div);

                if (urlMatches.Count > 0)
                {
                    resultIndicesContainingSmokeBallUrl.Add(i + 1);
                }
            }
            
            Console.WriteLine(string.Join(", ", resultIndicesContainingSmokeBallUrl));
        }
    }
}
