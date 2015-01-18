using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace com.DAL.Base
{
    /// <summary>
    /// 缓存操作类
    /// 2008-07-01 Created By Handey Pan
    /// </summary>
    public class BaseCacheDAL
    {
        public static Object lockobj = new object();
        //public delegate Object Filter();
        private readonly ICacheManager cacheManager;

        public BaseCacheDAL(string cacheName)
        {
            cacheManager = CacheFactory.GetCacheManager(cacheName);
        } 

        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object GetCache(string key)
        {
            return cacheManager.GetData(key);
        }

        

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存关键词</param>
        /// <param name="value">缓存值</param>
        /// <param name="timeout">缓存失效时间，分钟为单位(0为永久不失效)</param>
        public void SetCache(string key, Object value, int timeout)
        {
            if (timeout == 0)
            {
                cacheManager.Add(key, value, CacheItemPriority.High, null, new NeverExpired());
            }
            else
            {
                cacheManager.Add(key, value, CacheItemPriority.High, null,
                                 new SlidingTime(TimeSpan.FromMinutes(timeout)));
            }
        }

        /// <summary>
        /// 清除指定缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (cacheManager.Contains(key))
            { 
                cacheManager.Remove(key); 
            }
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void RemoveAll()
        { 
            cacheManager.Flush();
        }
    }
}
