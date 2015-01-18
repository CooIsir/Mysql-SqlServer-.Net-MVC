//------------------------------------------------------------------------------
// <copyright company="Telligent Systems">
//     Copyright (c) Telligent Systems Corporation.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;

namespace com.Utility.HttpModule
{ 
    /// <summary>
    /// This HttpModule encapsulates all the forums related events that occur 
    /// during ASP.NET application start-up, errors, and end request.
    /// </summary>
    // ***********************************************************************/
    public class bbsHttpModule : IHttpModule
    {
        #region Member variables and inherited properties / methods
        /// <summary>
        /// Initializes the HttpModule and performs the wireup of all application
        /// events.
        /// </summary>
        /// <param name="application">Application the module is being run for</param>
        public void Init(HttpApplication application)
        { 
            application.BeginRequest += new EventHandler(this.Application_BeginRequest); 
        } 
        public void Dispose() { }
        #endregion

        #region Application BeginRequest
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            ReWriteUrl(application.Context); 
        }
        #endregion Application BeginRequest
         
		#region ReWriteUrl
        /// <summary>
        /// 实现url重写
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool ReWriteUrl(HttpContext context)
        {
            string path = context.Request.Url.PathAndQuery;
            string fcode, tid, page, state, sHost,uid,ucode;
            Match mc;

            // 查看贴子
            mc = Regex.Match(path, @"/(?<host>[\w]+)/topic/(?<tid>[\d]+)(-(?<page>[\d]+))?.html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                sHost = mc.Groups["host"].Value;
                tid = mc.Groups["tid"].Value;
                page = mc.Groups["page"].Value;

                if (File.Exists(context.Server.MapPath(string.Format("/aspx/{0}/topic.aspx", sHost))))
                    context.RewritePath(string.Format("/aspx/{0}/topic.aspx?tid={1}&pn={2}", sHost, tid, page));
                else
                    context.RewritePath(string.Format("/aspx/default/topic.aspx?tid={0}&pn={1}", tid, page));
                return true;
            }
            
            // 版块页
            mc = Regex.Match(path, @"/(?<host>[\w]+)/forum/(?<fid>[\d]+)-(?<state>[\d]+)-(?<page>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                sHost = mc.Groups["host"].Value;
                fcode = mc.Groups["fid"].Value;
                page = mc.Groups["page"].Value;
                state = mc.Groups["state"].Value;
                if (File.Exists(context.Server.MapPath(string.Format("/aspx/{0}/forum.aspx", sHost))))
                    context.RewritePath(string.Format("/aspx/{0}/forum.aspx?fid={1}&pn={2}&state={3}", sHost, fcode, page, state));
                else
                    context.RewritePath(string.Format("/aspx/default/forum.aspx?fid={0}&pn={1}&state={2}", fcode, page, state));
                return true;
            }
            // 旧版块页
            mc = Regex.Match(path, @"/board-(?<fid>228).aspx", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                fcode = mc.Groups["fid"].Value;
                context.RewritePath(string.Format("/aspx/default/forum.aspx?fid={0}&pn=1&state=1", fcode));
                return true;
            }
            // bbs总首页
            if (path.ToLower() == "/" || path.ToLower() == "/index.html")
            {
                context.RewritePath("/aspx/default/index.aspx");
                return true;
            }

            // 搜索页
            //if (path.ToLower() == "/so")
            //{
            //    return false;
            //}

            mc = Regex.Match(path, @"/so+(/index.html)", RegexOptions.IgnoreCase);
            // 搜索页
            if (mc.Success && path.IndexOf(".asmx") < 0)
            {
                context.RewritePath(@"/aspx/search.aspx");
                return true;
            }

            // 分首页
            mc = Regex.Match(path, @"/(?<host>[a-zA-Z0-9]+)/index.html?$", RegexOptions.IgnoreCase);
            if (mc.Success && path.IndexOf(".asmx") < 0)
            {
                sHost = mc.Groups["host"].Value;
                if (File.Exists(context.Server.MapPath(string.Format("/aspx/{0}/index.aspx", sHost))))
                    context.RewritePath(string.Format("/aspx/{0}/index.aspx?fcode={0}", sHost));
                else
                    context.RewritePath(string.Format("/aspx/default/index.aspx?fcode={0}", sHost));
                return true;
            }
            mc = Regex.Match(path, @"/user/(?<uid>[\d]+)-(?<page>[\d]+)-(?<postpage>[\d]+).html", RegexOptions.IgnoreCase);
            // 用户详细页
            if (mc.Success)
            {
                tid = mc.Groups["uid"].Value;
                page = mc.Groups["page"].Value;
                string postPage = mc.Groups["postpage"].Value;
                context.RewritePath(string.Format("/aspx/userinfo.aspx?uid={0}&pn={1}&ppn={2}", tid, page, postPage));
                return true;
            }
            // 用户资料详细页
            //Create by xuzhibin 2008-06-27
            mc = Regex.Match(path, @"/user/(?<uid>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                tid = mc.Groups["uid"].Value;

                context.RewritePath(string.Format("/aspx/userview.aspx?userid={0}", tid));
                return true;
            }
            // 用户发表的帖子
            //Create by xuzhibin 2008-06-27
            mc = Regex.Match(path, @"/user/usertopics-(?<userid>[\d]+)-(?<page>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                uid = mc.Groups["userid"].Value;
                page = mc.Groups["page"].Value;
                context.RewritePath(string.Format("/aspx/usertopics.aspx?userid={0}&index={1}", uid, page));
                return true;
            }

            // 用户回复的帖子
            //Create by xuzhibin 2008-06-27
            mc = Regex.Match(path, @"/user/userreply-(?<userid>[\d]+)-(?<page>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                uid = mc.Groups["userid"].Value;
                page = mc.Groups["page"].Value;
                context.RewritePath(string.Format("/aspx/userreply.aspx?userid={0}&index={1}", uid, page));
                return true;
            }

            //mc = Regex.Match(path, @"[^39.net]+image\.aspx(?<par>\?[\s\S]*)", RegexOptions.IgnoreCase);
            //if (mc.Success)
            //{
            //    sHost = context.Request.Url.Host;
            //    context.RewritePath(string.Format("/image.aspx{0}", mc.Groups["par"].Value,sHost));                
            //    return true;
            //}

            #region 用户中心
            /**用户中心－－首页
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/usercp.html", RegexOptions.IgnoreCase);
            
            if (mc.Success)
            {
                context.RewritePath("/aspx/usercp.aspx");
                return true;
            }

            
            /**用户中心－－个人设置
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/usercpprofile.html", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                context.RewritePath("/aspx/usercpprofile.aspx");
                return true;
            }

            /**用户中心－－短消息--收件箱
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/usercpinbox(-(?<page>[\d]+))?.html", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                context.RewritePath(string.Format("/aspx/usercpinbox.aspx?index={0}",page));
                return true;
            }
            /**用户中心－－短消息--发件箱
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/usercpsentbox(-(?<page>[\d]+))?.html", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                context.RewritePath(string.Format("/aspx/usercpsentbox.aspx?index={0}", page));
                return true;
            }
            /**用户中心－－短消息--写新消息
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/usercppostpm(-(?<pmid>[\d]+))?.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                string pmid = mc.Groups["pmid"].Value;
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/usercppostpm.aspx?pmid={0}&{1}", pmid, query));
                return true;
            }

            /**用户中心－－短消息--查看消息
            * Create by xuzhibin 2008-06-30
            * */
            mc = Regex.Match(path, @"/user/usercpshowpm-(?<pmid>[\d]+).html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                string pmid=mc.Groups["pmid"].Value;
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/usercpshowpm.aspx?pmid={0}&{1}",pmid,query));
                return true;
            }

            /**用户中心－－我发表的主题
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/mytopics(-(?<page>[\d]+))?.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/mytopics.aspx?index={0}&{1}",page,query));
                return true;
            }

            /**用户中心－－我刚被回复的主题
            * Create by xuzhibin 2008-06-30
            * */
            mc = Regex.Match(path, @"/user/mynewposttopics(-(?<page>[\d]+))?.html", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                context.RewritePath(string.Format("/aspx/mynewposttopics.aspx?index={0}", page));
                return true;
            }

            /**用户中心－－收藏的主题
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/mycollectiontopic(-(?<page>[\d]+))?.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/mycollectiontopic.aspx?index={0}&{1}", page,query));
                return true;
            }

            /**用户中心－－收藏的板块
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/mycollectionforum(-(?<page>[\d]+))?.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/mycollectionforum.aspx?index={0}&{1}", page, query));
                return true;
            }
            /**用户中心－－添加收藏
            * Create by xuzhibin 2008-07-04
            * */
            mc = Regex.Match(path, @"/user/addcollection.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/addcollection.aspx?{0}", query));
                return true;
            }

            /**用户中心－－我的好友
             * Create by xuzhibin 2008-06-30
             * */
            mc = Regex.Match(path, @"/user/myfriends(-(?<page>[\d]+))?.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                page = string.IsNullOrEmpty(mc.Groups["page"].Value) ? "1" : mc.Groups["page"].Value;
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/myfriends.aspx?posttime={0}&index={1}&{2}", DateTime.Now.Ticks, page, query));
                return true;
            }
            /**用户中心－－添加好友
            * Create by xuzhibin 2008-06-30
            * */
            mc = Regex.Match(path, @"/user/addfriend.html(\?(?<query>[^/]+))?", RegexOptions.IgnoreCase);

            if (mc.Success)
            {
                string query = mc.Groups["query"].Value;
                context.RewritePath(string.Format("/aspx/addfriend.aspx?{0}", query));
                return true;
            }
            #endregion

            mc = Regex.Match(path, @"/so/index.html\?wd=(?<wd>[^&]+)(&pn=(?<pn>[\d]+))?", RegexOptions.IgnoreCase);
            // 搜索页
            if (mc.Success && path.IndexOf(".asmx")<0)
            {
                tid = mc.Groups["wd"].Value;
                page = mc.Groups["pn"].Value;
                context.RewritePath(string.Format(@"/aspx/search.aspx?wd={0}&pn={1}", tid, page));
                return true;
            }
            
            mc = Regex.Match(path, @"/tag/(?<wd>[^/]+)(/(?<pn>[\d]+))?", RegexOptions.IgnoreCase);
            // 标签页
            if (mc.Success)
            {
                tid = System.Web.HttpUtility.UrlDecode(mc.Groups["wd"].Value, System.Text.Encoding.UTF8);
                page = mc.Groups["pn"].Value;
                context.RewritePath(string.Format(@"/aspx/tag.aspx?wd={0}&pn={1}", System.Web.HttpUtility.UrlEncode(tid, System.Text.Encoding.GetEncoding("gb2312")), page));
                return true;
            }
            mc = Regex.Match(path, @"/useractivated/(?<uid>[\d]+)/(?<ucode>[\S]+).html", RegexOptions.IgnoreCase);
            //用户邮箱激活页
            if(mc.Success)
            {
                uid = mc.Groups["uid"].Value;
                ucode = mc.Groups["ucode"].Value;
                context.RewritePath(string.Format(@"/aspx/useractivated.aspx?uid={0}&ucode={1}", uid, ucode));
            }

            // 旧版贴子
            mc = Regex.Match(path, @"/topic-(?<tid>[\d]+).aspx", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                tid = mc.Groups["tid"].Value;
                context.RewritePath(string.Format("/aspx/default/topic.aspx?tid={0}&pn=1", tid));
                return true;
            }
            return false;
        }
        #endregion ReWriteUrl
    }
}
