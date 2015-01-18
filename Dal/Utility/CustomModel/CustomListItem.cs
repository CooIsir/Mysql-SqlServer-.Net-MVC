using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class CustomListItem
    {
        public CustomListItem()
        {

        }
        public CustomListItem(string sText,string sValue)
        {
            this._Text = sText;
            this._Value = sValue;
        }

        private string _Text = "";
        private string _Value = "";

        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}