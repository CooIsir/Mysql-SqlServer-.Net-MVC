using System;
using System.Collections.Generic;
using System.Text;
using System.Net ;

namespace com.Utility
{
    public class RequestArgs
    {
        #region ×Ö¶Î
        private string _Url = "";
        private string _RefererUrl = "";
        private string _postData = "";
        private string _cookie = "";
        private string _Encode = "UTF-8";
        private string _charset = "";
        private string _Method = "GET";
        private string _Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
        private string _IpAddress = "";
        private string _ContentType = "";
        private bool _blPolicy = false;
        private bool _blRedirect = false;
        private bool _blGetHeaders = false;
        private int _TimeOut = 5000;
        private List<CookieObj> _listCookie = new List<CookieObj>();
        #endregion ×Ö¶Î

        #region ÊôÐÔ
        public string Url
        {
            get { return _Url; ; }
            set { _Url = value; }
        }
        public string RefererUrl
        {
            get { return _RefererUrl; }
            set { _RefererUrl = value; }
        }
        public string postData
        {
            get { return _postData; }
            set { _postData = value; }
        }
        public string cookie
        {
            get
            {
                if (string.IsNullOrEmpty(_cookie))
                {
                    foreach (CookieObj mCookie in listCookie)
                    {
                        _cookie += string.Format("{0}={1};", mCookie.cookieName, mCookie.cookieValue);
                    }
                }
                return _cookie;
            }
            set { _cookie = value; }
        }

        public string Encode
        {
            get { return _Encode; }
            set { _Encode = value; }
        }
        public string charset
        {
            get { return _charset; }
            set { _charset = value; }
        }
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }
        public string IpAddress
        {
            get { return _IpAddress; }
            set { _IpAddress = value; }
        }
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        public bool blPolicy
        {
            get { return _blPolicy; }
            set { _blPolicy = value; }
        }
        public bool blRedirect
        {
            get { return _blRedirect; }
            set { _blRedirect = value; }
        }
        public bool blGetHeaders
        {
            get { return _blGetHeaders; }
            set { _blGetHeaders = value; }
        }
        public int TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        public  List<CookieObj> listCookie 
        {
            get{return _listCookie ;}
            set{_cookie ="";  _listCookie = value;}
        }
        #endregion ÊôÐÔ
    }
}
