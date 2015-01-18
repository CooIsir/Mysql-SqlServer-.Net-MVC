using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class CookieObj
    {
        private string _cookieName = "";
        private string _cookieValue = "";

        public  string cookieName  
        {
            get{ return _cookieName ; }
            set{_cookieName = value ; }
        
        }
        public string cookieValue 
        {
           get{ return _cookieValue ; }
           set{ _cookieValue = value;} 
        }
    }
}
