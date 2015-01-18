using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace com.Model.Base
{
    [DataContract]
    public class SearchArgs
    {
        private int _fid=0;
        private int _fnum1 = 0;
        private int _fnum2 = 0;
        private int _fnum3 = 0;
        private string _keyWord;
        private List<string> _listField = new List<string>(){ "Title"} ;
        private int _pageSize=10;
        private int _pageIndex=1;
        private string _sortField= "CreateOn";
        private string _searchPath = "";
        private bool _reverse = true;

        [DataMember]
        public int fid
        {
            get { return _fid; }
            set { _fid = value; }
        }
        [DataMember]
        public int fnum1  
        {
            get{return _fnum1 ;}
            set { _fnum1 = value; }
        }
        [DataMember]
        public int fnum2  
        {
            get { return _fnum2; }
            set { _fnum2 = value; }
        }
        [DataMember]
        public int fnum3
        {
            get { return _fnum3; }
            set { _fnum3 = value; }
        }
        [DataMember]
        public string keyWord
        {
            get { return _keyWord; }
            set { _keyWord = value; }
        }
        [DataMember]
        public List<string> listField
        {
            get { return _listField; }
            set { _listField = value; }
        }
        [DataMember]
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        [DataMember]
        public int pageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }
        [DataMember]
        public string sortField
        {
            get { return _sortField; }
            set { _sortField = value; }
        }
        [DataMember]
        public string searchPath
        {
            get { return _searchPath; }
            set { _searchPath = value; }
        }
        [DataMember]
        public bool reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        } 
    }
}