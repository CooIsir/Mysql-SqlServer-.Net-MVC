using System;
using System.Collections.Generic;
using System.Text;

namespace com.jk39.Utility
{
    public class CDateTime
    {
        public static string GetPath(DateTime dt)
        {
            return dt.Year.ToString() + dt.Month.ToString("00")+"/"+dt.Day.ToString("00")+"/";
        }
    }
}
