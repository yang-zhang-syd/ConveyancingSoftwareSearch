using System;

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
