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
            IHtmlParser parser = new SimpleHtmlParser();

            var findSmokeBall = new FindSmokeBall(webRequestSender, parser);
            var resultIndicesContainingSmokeBallUrl = findSmokeBall.Run(url);
            var result = resultIndicesContainingSmokeBallUrl.Count > 0
                ? string.Join(", ", resultIndicesContainingSmokeBallUrl)
                : "0";
            Console.WriteLine(result);
        }
    }
}
