using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.Utility
{
    public  class IpModel
    {
        #region 私有成员
        private string ip;
        private string country;
        private string local;
        private long firstStartIp = 0;
        private long lastStartIp = 0;       
        private long startIp = 0;
        private long endIp = 0;
        private int countryFlag = 0;
        private long endIpOff = 0;
        private string errMsg = null;
        private int _nRet = 0;
        private double duration = 0.0;
        #endregion
          
        #region 公共属性 
        public string IP
        {
            get { return ip; }
            set { ip = value; }            
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        public string Local
        {
            get { return local; }
            set { local = value; }
        }
        public long FirstStartIp 
        {
            get { return firstStartIp; }
            set { firstStartIp = value; }
        }
        public long LastStartIp 
        {
            get { return lastStartIp; }
            set { lastStartIp = value; }
        }
        public long StartIp  
        {
            get { return startIp; }
            set { startIp = value; }
        }
        public long EndIp 
        {
            get { return endIp; }
            set { endIp = value; }
        }
        public int CountryFlag 
        {
            get { return countryFlag; }
            set { countryFlag = value; }
        }
        public long EndIpOff
        {
            get { return endIpOff; }
            set { endIpOff = value; }
        }

        public string ErrMsg
        {
            get { return errMsg; }
            set { errMsg = value; }
        }
        public int nRet
        {
            get { return _nRet; }
            set { _nRet = value; }
        }
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        #endregion
    }
}
