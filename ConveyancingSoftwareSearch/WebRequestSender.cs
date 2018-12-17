using System.IO;
using System.Net;

namespace ConveyancingSoftwareSearch
{
    public interface IWebRequestSender
    {
        string Get(string url);
    }

    public class WebRequestSender : IWebRequestSender
    {
        public string Get(string url)
        {
            var request = WebRequest.CreateHttp(url);
            var responseStream = request.GetResponse().GetResponseStream();

            var sr = new StreamReader(responseStream);
            var response = sr.ReadToEnd();
            return response;
        }
    }
}
