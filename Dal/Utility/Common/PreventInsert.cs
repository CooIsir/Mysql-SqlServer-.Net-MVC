using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Utility
{
    public class PreventInsert
    {
        public static bool IsInsert(string s)
        {
            string sql = "exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare |drop |creat ";
            string[] sql_c = sql.Split('|');
            foreach (var sl in sql_c)
            {
                if (s.ToLower().IndexOf(sl) >= 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
