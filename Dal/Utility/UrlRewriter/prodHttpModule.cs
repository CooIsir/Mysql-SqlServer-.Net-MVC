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
    public class prodHttpModule : IHttpModule
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
            string fcode, id, page, state, sHost, uid, ucode;

            Match mc = Regex.Match(path, @"/prod/(?<id>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                id = mc.Groups["id"].Value;
                context.RewritePath(string.Format("/aspx/product.aspx?id={0}", id));
                return true;
            }

            mc = Regex.Match(path, @"/list/(?<id>[\d]+)-(?<page>[\d]+).html", RegexOptions.IgnoreCase);
            if (mc.Success)
            {
                id = mc.Groups["id"].Value;
                page = mc.Groups["page"].Value;
                context.RewritePath(string.Format("/aspx/categorylist.aspx?id={0}&pn={1}", id,page));
                return true;
            }

            // bbs总首页
            if (path.ToLower() == "/" || path.ToLower() == "/index.html")
            {
                context.RewritePath("/aspx/index.aspx");
                return true;
            }
            return false;
        }
        #endregion ReWriteUrl
    }
}