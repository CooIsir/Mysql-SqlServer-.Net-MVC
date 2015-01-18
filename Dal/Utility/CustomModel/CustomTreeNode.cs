using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class CustomTreeNode
    {
        public CustomTreeNode() { }     
        private string _Text = "";
        private string _Value = "";
        private string _ParentID = "";

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
        public string ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }
    }
}
