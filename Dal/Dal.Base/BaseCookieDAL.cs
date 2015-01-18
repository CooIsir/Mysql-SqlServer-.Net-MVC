using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.Model.Base;
using com.Utility;

namespace com.DAL.Base
{
    public class BaseCookieDAL<T> where T: BaseCookieModel , new ()
    {
        public virtual T GetCookie()
        {
            T model = new T();
            int i = 0;
            int.TryParse(cookieHelper.GetCookie("money"), out i);
            model.money = i;
            model.picurl = System.Web.HttpUtility.UrlDecode(cookieHelper.GetCookie("picurl"), Encoding.UTF8);
            i = 0;
            int.TryParse(cookieHelper.GetCookie("pid"), out i);
            model.pid = i;
            model.username = System.Web.HttpUtility.UrlDecode(cookieHelper.GetCookie("username"), Encoding.UTF8);
            model.nickname = System.Web.HttpUtility.UrlDecode(cookieHelper.GetCookie("nickname"), Encoding.UTF8);
            long l = 0;
            long.TryParse(cookieHelper.GetCookie("verify"), out l);
            model.verify = l;

            long.TryParse(cookieHelper.GetCookie("onlineId"), out l);
            model.onlineId = l;

            return model;
        }

        public virtual  void SetCookie(T model)
        {
            if (model.isnew)
            {
                cookieHelper.SetCookie("money", model.money.ToString());
                cookieHelper.SetCookie("picurl", System.Web.HttpUtility.UrlEncode(model.picurl));
                cookieHelper.SetCookie("pid", model.pid.ToString());
                cookieHelper.SetCookie("username", System.Web.HttpUtility.UrlEncode(model.username, Encoding.UTF8));
                cookieHelper.SetCookie("nickname", System.Web.HttpUtility.UrlEncode(model.nickname, Encoding.UTF8));
                cookieHelper.SetCookie("verify", model.verify.ToString());
                cookieHelper.SetCookie("onlineId", model.onlineId.ToString());
            }
        }

        public virtual void LogOut(T model)
        {
            cookieHelper.SetCookie("money", "", -1);
            cookieHelper.SetCookie("picurl", "", -1);
            cookieHelper.SetCookie("pid", "", -1);
            cookieHelper.SetCookie("username", "", -1);
            cookieHelper.SetCookie("nickname", "", -1);
            cookieHelper.SetCookie("verify", "", -1);
            cookieHelper.SetCookie("onlineId","", -1);
        }
    }
}