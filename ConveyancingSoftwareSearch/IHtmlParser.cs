using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConveyancingSoftwareSearch
{
    public interface IHtmlParser
    {
        List<int> GetMatchIndices(Regex regex);
        string ReadDivAtIndex(int index);
        void SetHtml(string html);
    }
}