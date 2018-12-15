using System;
using System.Collections.Generic;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public class HtmlTag : IHtmlElement
    {
        public HtmlTag()
        {
            Children = new List<IHtmlElement>();
        }

        public string TagName { get; set; }

        public HtmlAttribute Attribute { get; set; }

        public List<IHtmlElement> Children { get; set; }
    }
}
