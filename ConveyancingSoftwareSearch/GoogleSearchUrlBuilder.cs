using System.Web;

namespace ConveyancingSoftwareSearch
{
    public static class GoogleSearchUrlBuilder
    {
        public static string Build(string keyword, int numOfResults)
        {
            return $"http://www.google.com/search?q={HttpUtility.UrlEncode(keyword)}&num={numOfResults}";
        }
    }
}
