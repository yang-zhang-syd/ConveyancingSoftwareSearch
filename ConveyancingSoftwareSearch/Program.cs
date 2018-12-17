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
            
            var uriString = $"http://www.google.com/search?q={HttpUtility.UrlEncode(keywordString)}&num={numOfResults}";
            
            var request = WebRequest.CreateHttp(uriString);
            var responseStream = request.GetResponse().GetResponseStream();

            var sr = new StreamReader(responseStream);
            var response = sr.ReadToEnd();

            var regex = new Regex("<div class=\"g\">");
            var matches = regex.Matches(response);
            var resultsBeginIndexes = matches
                .Select(m => m.Index)
                .OrderBy(i => i)
                .ToList();

            //foreach (var idx in resultsBeginIndexes)
            //{
            //    Console.WriteLine(response.Substring());
            //}

            regex = new Regex(@"www\.smokeball\.com\.au");
            matches = regex.Matches(response);
            var smokeballBeginIndexes = matches
                .Select(m => m.Index)
                .OrderBy(i => i)
                .ToList();


            foreach (var idx in smokeballBeginIndexes)
            {
                Console.WriteLine(response.Substring(idx, 100));
            }

            var resultsIndexes = new List<int>();
            foreach (var sidx in smokeballBeginIndexes)
            {
                var resultBeginAt = resultsBeginIndexes.First(i => i > sidx);
                var resultIndex = resultsBeginIndexes.IndexOf(resultBeginAt);
                resultsIndexes.Add(resultIndex);
            }

            resultsIndexes = resultsIndexes.Distinct().OrderBy(i => i).ToList();

            Console.WriteLine(string.Join(',', resultsIndexes));
        }
    }
}
