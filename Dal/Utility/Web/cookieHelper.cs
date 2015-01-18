using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace com.Utility
{
    public class cookieHelper
    {
        /// <summary>
        /// 获取用户cookie值
        /// </summary>
        public static string GetCookie(string name)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpCookie cookie = request.Cookies[name];
            return (cookie != null ? cookie.Value : "");
        }

        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, 500000);
        }

        /// <summary>
        /// 设置用户cookie名,值，有效时间，以秒为单位
        /// </summary>
        public static void SetCookie(string name, string value, int iMinutes)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            HttpCookie mycookie = new HttpCookie(name);
            mycookie.Value = value;
            mycookie.Expires = DateTime.Now.AddMinutes(iMinutes);
            //string strDomain = System.Configuration.ConfigurationManager.AppSettings["domain"];
            //if (string.IsNullOrEmpty(strDomain))
            //{
            //    mycookie.Domain = ".39.net";
            //}
            //else
            //{
            //    mycookie.Domain = strDomain;
            //}
            response.Cookies.Add(mycookie);
        }

        public static string GetCookieByHead(string sInput)
        {
            string strCookie = CRegex.GetText(sInput, @"Set-Cookie:(?<Cookie>[\s\S]+?)\n", "Cookie");
            List<string> list = CRegex.GetList(strCookie, @"(?<cookie>[\w\d\.]+=[\w\d\._-]+);", "cookie");
            string sReturnCookie = "";
            foreach (string s in list)
            {
                sReturnCookie += s + "; ";
            }
            return sReturnCookie;
        }
        public static List<CookieObj> GetCookieList(string sInput, List<CookieObj> listInput)
        {
            string strCookie = CRegex.GetText(sInput, @"Set-Cookie:(?<Cookie>[\s\S]+?)\n", "Cookie");
            strCookie = CRegex.Replace(strCookie, @"expires=([^;]+)GMT;", "", 0);
            strCookie = strCookie.Replace("path=/", "");
            List<string> list = CRegex.GetList(strCookie, @"(?<cookie>[\w\d\&\.=]+[\w\d\._-]+);", "cookie");
            string cookieName, cookieValue;
            CookieObj mCookie = null;
            List<CookieObj> listCookie = new List<CookieObj>();
            foreach (string s in list)
            {
                if (s.IndexOf('=') < 1)
                    continue;

                cookieName = s.Substring(0, s.IndexOf('='));
                if (cookieName == "domain")
                    continue;
                if (s.Length < s.IndexOf('=') + 1)
                    cookieValue = "";
                else
                    cookieValue = s.Substring(s.IndexOf('=') + 1);
                mCookie = new CookieObj();
                mCookie.cookieName = cookieName;
                mCookie.cookieValue = cookieValue;
                listCookie.Add(mCookie);
            }
            bool blExists;
            CookieObj mInput;
            foreach (CookieObj model in listCookie)
            {
                blExists = false;
                for (int i = 0; i < listInput.Count; i++)
                {
                    mInput = listInput[i];
                    if (mInput.cookieName == model.cookieName)
                    {
                        blExists = true;
                        mInput.cookieValue = model.cookieValue;
                    }
                }
                if (!blExists)
                {
                    listInput.Add(model);
                }
            }
            return listInput;
        }
        public static void SetCookie(List<CookieObj> listInput, string CookieName, string CookieValue)
        {
            foreach (CookieObj mCookie in listInput)
            {
                if (mCookie.cookieName == CookieName)
                {
                    mCookie.cookieValue = CookieValue;
                    return;
                }
            }
            CookieObj obj = new CookieObj();
            obj.cookieName = CookieName;
            obj.cookieValue = CookieValue;
            listInput.Add(obj);
        }
        public static void Remove(List<CookieObj> listInput, string CookieName)
        {
            for (int i = 0; i < listInput.Count; i++)
            {
                if (listInput[i].cookieName == CookieName)
                {
                    listInput.RemoveAt(i);
                }
            }
        }

    }
}
