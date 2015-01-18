using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class FiledItem
    {
        private string _FN = "";
        private string _DN = "";
        private string _TT = "";
        private string _FT = "";
        private int _FL = 10;
        private bool _NL = true;

        public string FN
        {
            get { return _FN; }
            set { _FN = value; }
        }
        public string DN
        {
            get { return _DN; }
            set { _DN = value; }
        }
        public string TT
        {
            get { return _TT; }
            set { _TT = value; }
        }
        public string FT
        {
            get { return _FT; }
            set { _FT = value; }
        }
        public int FL
        {
            get { return _FL; }
            set { _FL = value; }
        }
        public bool NL
        {
            get { return _NL; }
            set { _NL = value; }
        }
    }
}
