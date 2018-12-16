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
            IsSelfClosed = false;
        }

        public string TagName { get; set; }

        public bool IsSelfClosed { get; set; }

        public HtmlAttribute Attributes { get; set; }

        public List<IHtmlElement> Children { get; set; }
    }
}
