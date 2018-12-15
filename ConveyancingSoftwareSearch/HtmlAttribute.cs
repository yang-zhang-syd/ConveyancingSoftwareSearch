using System;
using System.Collections.Generic;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    /// <summary>
    /// This is a simplified html attribute which keep everything as a single string.
    /// </summary>
    public class HtmlAttribute
    {
        public HtmlAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
