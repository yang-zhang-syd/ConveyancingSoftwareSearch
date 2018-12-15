using System;
using System.Collections.Generic;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public class InnerText : IHtmlElement
    {
        public InnerText(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
