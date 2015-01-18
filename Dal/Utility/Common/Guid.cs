using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Utility
{
    public class CGuid
    { 
        public static long GetNewId()
        {
            return BitConverter.ToInt64(System.Guid.NewGuid().ToByteArray(), 0) + DateTime.Now.Ticks % (720000000000000);
        }
    }
}
