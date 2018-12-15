using System;
using System.Collections.Generic;
using System.Text;

namespace ConveyancingSoftwareSearch
{
    public class ResultAndNextRead<T>
    {
        public ResultAndNextRead(T t, int next)
        {
            Result = t;
            NextRead = next;
        }

        public T Result { get; set; }
        public int NextRead { get; set; }
    }
}
