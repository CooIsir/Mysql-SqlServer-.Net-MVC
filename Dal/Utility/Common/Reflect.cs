using System;
using System.Reflection;
using System.Configuration;

namespace com.Utility
{
    public partial class Reflect<T> where T : class
    {
        private static readonly string sPath = "ITDB.DAL";
        private static System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;

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
            try
            { 
                objType = (T)CreateAssembly(sFilePath).CreateInstance(CacheKey);//反射创建 
            }
            catch (Exception Ex) { string str = Ex.Message; }
            return objType;
        }

        public static Assembly CreateAssembly(string sFilePath)
        {
            Assembly assObj = (Assembly)objCache[sFilePath];
            if (assObj == null)
            {
                assObj = Assembly.Load(sFilePath);
                objCache.Insert(sFilePath, assObj);
            }
            return assObj;
        }
    }
}

