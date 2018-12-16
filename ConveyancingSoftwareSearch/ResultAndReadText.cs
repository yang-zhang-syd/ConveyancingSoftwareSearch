using System;
using System.Collections.Generic;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public class ResultAndReadText<T>
    {
        public ResultAndReadText(T t, string readText)
        {
            Result = t;
            ReadText = readText;
        }

        public T Result { get; set; }
        public string ReadText { get; set; }
    }
}
