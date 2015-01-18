using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Utility
{
    public class MathTools
    {
        //当为百分位时，不进位，否则百分位无条件进位
        public static double Round(double value, int digit)
        {
            double r;
            string s = value.ToString();
            int re = s.Length - s.IndexOf('.') - 1;
            if (re > 2)
            {
                double vt = Math.Pow(10, digit + 1);
                r = Math.Round(value + 5 / vt, digit);
            }
            else
            {
                r = value;
            }
            return r;
        }
    }
}
