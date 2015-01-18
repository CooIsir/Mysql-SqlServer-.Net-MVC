using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Model.Base
{
    public class BaseCookieModel
    {
        public BaseCookieModel()
        {

        }

        private int _pid = 0;
        private long _verify = 0;
        private long _onlineId = 0;
        private string _username = "";
        private string _nickname = "";
        private int _money = 0;
        private string _picurl = "";
        private bool _isnew = false;

        /// <summary>
        /// 通行证id
        /// </summary>
        public int pid
        {
            get { return _pid; }
            set { _pid = value; }
        }
        /// <summary>
        /// 通行证校验码
        /// </summary>
        public long verify
        {
            get { return _verify; }
            set { _verify = value; }
        }
        public long onlineId
        {
            get { return _onlineId; }
            set { _onlineId = value; }
        }
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        public int money
        {
            get { return _money; }
            set { _money = value; }
        }
        public string picurl
        {
            get { return _picurl; }
            set { _picurl = value; }
        }
        public bool isnew
        {
            get { return _isnew; }
            set { _isnew = value; }
        }
    }
}