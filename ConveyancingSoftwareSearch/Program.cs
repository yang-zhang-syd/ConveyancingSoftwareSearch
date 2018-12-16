using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace ConveyancingSoftwareSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var keywordString = "conveyancing software";
            var numOfResults = 100;
            
            var uriString = $"http://www.google.com/search?q={HttpUtility.UrlEncode(keywordString)}&num={numOfResults}";
            
            var request = WebRequest.CreateHttp(uriString);
            var responseStream = request.GetResponse().GetResponseStream();

            var parser = new SimpleHtmlParser();
            var tag = parser.ParseHtml(responseStream);

            Console.Read();
        }
    }
}
