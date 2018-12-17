using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Moq;
using Xunit;

namespace ConveyancingSoftwareSearch.Test
{
    public class FindSmokeBallTest
    {
        private readonly IWebRequestSender _webRequestSender;
        private readonly IHtmlParser _htmlParser;

        public FindSmokeBallTest()
        {
            var webRequestSenderMock = new Mock<IWebRequestSender>();
            webRequestSenderMock.Setup(x => x.Get(It.IsAny<string>()))
                .Returns("");

            _webRequestSender = webRequestSenderMock.Object;

            var htmlParserMock = new Mock<IHtmlParser>();
            htmlParserMock.Setup(x => x.GetMatchIndices(It.IsAny<Regex>()))
                .Returns(new List<int> {1000, 2000, 3000});
            htmlParserMock.Setup(x => x.ReadDivAtIndex(1000))
                .Returns("<div></div>");
            htmlParserMock.Setup(x => x.ReadDivAtIndex(2000))
                .Returns("<div>www.smokeball.com.au</div>");
            htmlParserMock.Setup(x => x.ReadDivAtIndex(3000))
                .Returns("<div></div>");
            htmlParserMock.Setup(x => x.SetHtml(It.IsAny<string>()));

            _htmlParser = htmlParserMock.Object;
        }

        [Fact]
        public void Test1()
        {
            var keywordString = "conveyancing software";
            var numOfResults = 100;
            var url = GoogleSearchUrlBuilder.Build(keywordString, numOfResults);

            var findSomkeBall = new FindSmokeBall(_webRequestSender, _htmlParser);
            var result = findSomkeBall.Run(url);

            Assert.Equal(1, result.Count);
            Assert.True(result.Contains(2));
        }
    }
}
