using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace com.jk39.Utility
{
    public class global
    {
        public static readonly System.Web.Caching.Cache cacheManager = System.Web.HttpRuntime.Cache;
    }
}
