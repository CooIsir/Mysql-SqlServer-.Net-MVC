using System;
using System.Reflection;
using System.Configuration;
using System.Web;
 
namespace com.Utility
{ 
    public  class CReflect<T> where T:class   
    { 
        private static string sPath = "com"; 
        public static T Create(string sName)
        {
            return Create(sName, sPath); 
        }
        public static T Create(string sName, string sFilePath)
        {
            return Create(sName, sFilePath, true); 
        }
        public static T Create(string sName, string sFilePath, bool bCache)
        {
            string CacheKey = sFilePath + "." + sName;
            T objType = null;
            objType = (T)CacheUtility.GetAsembly(sFilePath).CreateInstance(CacheKey); //∑¥…‰¥¥Ω® 
            return objType;
        } 
    }
}

