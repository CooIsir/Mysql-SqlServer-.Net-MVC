using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class FileEntity
    {
        private string _FilePath = "";
        private string _FileName = "";
        private string _FileContent = "";

        public  string FilePath 
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }
        public string FileName 
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        public string FileContent
        {
            get { return _FileContent; }
            set { _FileContent = value; }
        }
    }
}
