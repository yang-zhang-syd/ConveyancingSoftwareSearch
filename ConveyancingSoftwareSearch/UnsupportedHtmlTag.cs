using System;
using System.Collections.Generic;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public class UnsupportedHtmlTag : IHtmlElement
    {
        public UnsupportedHtmlTag(string name, string value)
        {
            TagName = name;
            Value = value;
        }

        public string TagName { get; set; }
        public string Value { get; set; }
    }
}
