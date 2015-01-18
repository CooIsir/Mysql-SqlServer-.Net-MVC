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
    public class sosoHttpModule : IHttpModule
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
            string path = context.Request.Path.ToLower();
            Match mc;
            string fcode, tid, state, page; 

            // 贴子页
            mc = Regex.Match(path, @"/(?<tid>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                tid = mc.Groups["tid"].Value;
                context.RewritePath(string.Format("/default/topic.aspx?tid={0}", tid));
            }
            // 版块页
            mc = Regex.Match(path, @"/list/(?<fnum>[\d]+)-(?<state>[\d]{1,2})-(?<page>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                fcode = mc.Groups["fnum"].Value;
                state = mc.Groups["state"].Value;
                page = mc.Groups["page"].Value;
                context.RewritePath(string.Format("/default/forum.aspx?fnum={0}&state={1}&pn={2}", fcode, state, page));
                return true;
            }
            // 首页
            if (path.ToLower() == "/" || path.ToLower() == "/index.html")
            {
                context.RewritePath("/default/index.aspx");
                return true;
            }
            return false;
        }     
        #endregion ReWriteUrl
    }
}
