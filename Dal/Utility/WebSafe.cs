using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Utility
{
    public class WebSafe
    {
        public static bool SqlInsert(string strS)
        {
            bool b = true;
            string sql = "exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare |drop |creat ";
            string[] sql_c = sql.Split('|');
            foreach (var sl in sql_c)
            {
                if (strS.ToLower().IndexOf(sl) >= 0)
                {
                    b = false;
                }
            }
            return b;
        }
    }
}
