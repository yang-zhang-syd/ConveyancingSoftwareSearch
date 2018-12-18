using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConveyancingSoftwareSearch
{
    public class FindSmokeBall
    {
        private readonly IWebRequestSender _webRequestSender;
        private readonly IHtmlParser _htmlParser;

        public FindSmokeBall(IWebRequestSender webRequestSender, IHtmlParser htmlParser)
        {
            _webRequestSender = webRequestSender;
            _htmlParser = htmlParser;
        }

        public List<int> Run(string url)
        {
            var html = _webRequestSender.Get(url);
            _htmlParser.SetHtml(html);
            var matchIndices = _htmlParser.GetMatchIndices(new Regex("<div class=\"g\">"));
            var resultIndicesContainingSmokeBallUrl = new List<int>();

            for (int i = 0; i < matchIndices.Count; ++i)
            {
                var div = _htmlParser.ReadDivAtIndex(matchIndices[i]);

                var smokeBallUrlRegex = new Regex(@"www\.smokeball\.com\.au");
                var urlMatches = smokeBallUrlRegex.Matches(div);

                if (urlMatches.Count > 0)
                {
                    resultIndicesContainingSmokeBallUrl.Add(i + 1);
                }
            }

            return resultIndicesContainingSmokeBallUrl;
        }
    }
}
