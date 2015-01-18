using System;
using System.Collections.Generic;
using System.Text;

namespace com.DAL.Base
{
    public class dbParam
    {
        private string _ParamName = "";
        private System.Data.DbType _ParamDbType = System.Data.DbType.String;
        private object _ParamValue = null;

        public string ParamName
        {
            get { return _ParamName; }
            set { _ParamName = value; }
        }
        public System.Data.DbType ParamDbType
        {
            get { return _ParamDbType; }
            set { _ParamDbType = value; }
        }
        public object ParamValue
        {
            get { return _ParamValue; }
            set { _ParamValue = value; }
        }
    }
}
