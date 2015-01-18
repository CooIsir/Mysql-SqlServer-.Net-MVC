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
    public class BlogHttpModule : IHttpModule
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
            try
            {
                string path = context.Request.Path;
                string url = context.Request.Path;

                //泛域名支持
                //Regex regex = new Regex(@"^(\w+)\.blog\.39\.net$", RegexOptions.IgnoreCase);
                Regex regexNoMatch = new Regex(@"(\.gif|\.js|\.jpg|\.png|\.bmp|\.zip|\.jpeg|\.jpe|\.css|\.rar|\.xsl)$", RegexOptions.IgnoreCase);
                if (regexNoMatch.IsMatch(path))
                {
                    return false;
                }

                //清空上下文数据
                context.Items.Clear();

                Match mc;

                //// 博客首页/index.htm or /index.html
                //mc = Regex.Match(path, @"/index.(htm|html)$", RegexOptions.IgnoreCase);
                //if (mc.Success)
                //{
                //    context.RewritePath(string.Format("/index.aspx"));
                //    context.Items.Add("Path", path);
                //    return true;
                //}

                // 特殊处理（供目录浏览）
                mc = Regex.Match(path, "/39pages", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    return false;
                }

                // 首页，http://blog.39.net/test or http://blog.39.net/test/
                mc = Regex.Match(path, @"^/(\w|/)+$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    if (!path.EndsWith("/"))
                        path += "/";

                    context.RewritePath(string.Format("/default.aspx"));
                    context.Items.Add("Controls", "HomePage.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //非aspx、htm和html文件不经过往下的匹配
                if (!(path.Contains(".aspx") || path.Contains(".html") || path.Contains(".htm") || path.Contains(".xml") || path.Contains(".do")))
                {
                    return false;
                }

                // 管理后台首页
                mc = Regex.Match(path, @"^/(\w|/)+/admin/default.(html|aspx|htm)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath(string.Format("/admin/default.aspx"));
                    context.Items.Add("Path", path);
                    return true;
                }

                //栏目列表页
                mc = Regex.Match(path, @"^/(\w|/)+/blog_list.html$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath(string.Format("/blog_list.aspx"));
                    context.Items.Add("Path", path);
                    return true;
                }

                // 首页, http://blog.39.net/test.aspx or http://blog.39.net/test.html
                mc = Regex.Match(path, @"^/(\w|/)+/default+[_]*[\d]*.(html|aspx|htm)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath(string.Format("/default.aspx"));
                    context.Items.Add("Controls", "HomePage.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //文章, http://blog.39.net/archive/2008/04/10/1.html
                mc = Regex.Match(path, @"/archive/\d{4}/\d{2}/\d{2}/\w+\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Article.aspx");
                    context.Items.Add("Controls", "ViewPost.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 日期(天)档案, http://blog.39.net/archive/2008/04/10.html or http://blog.39.net/archive/2008/04/10_2.html
                mc = Regex.Match(path, @"/archive/\d{4}/\d{2}/\w{1,}\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Archive.aspx");
                    context.Items.Add("Controls", "ArchiveDay.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //日期(月)档案, http://blog.39.net/archive/2008/04.html or http://blog.39.net/archive/2008/04_2.html
                mc = Regex.Match(path, @"/archive/\d{4}/\w{1,}\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Archive.aspx");
                    context.Items.Add("Controls", "ArchiveMonth.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //日期(年)档案, http://blog.39.net/archive/2008.html or http://blog.39.net/archive/2008_2.html
                mc = Regex.Match(path, @"/archive/\w{1,}\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Archive.aspx");
                    context.Items.Add("Controls", "ArchiveYear.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 相册, http://blog.39.net/gallery/2992.html or http://blog.39.net/gallery/2992_1.html
                mc = Regex.Match(path, @"/gallery/\w{1,}\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/default.aspx");
                    context.Items.Add("Controls", "GalleryThumbNailViewer.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 图片
                mc = Regex.Match(path, @"/gallery/Image/\d{1,}\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/default.aspx");
                    context.Items.Add("Controls", "ViewPicture.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 留言
                mc = Regex.Match(path, @"(?<username>/\w+)/Contact.aspx$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/default.aspx");
                    context.Items.Add("Controls", "ArchiveMessage.ascx,Contact.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //收藏夹
                mc = Regex.Match(path, @"(?<username>/\w+)/Favourites_[\d]*_[\d]*\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Favourites.aspx");
                    context.Items.Add("Controls", "Favourites.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //好友
                mc = Regex.Match(path, @"(?<username>/\w+)/Friends.html$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Friends.aspx");
                    context.Items.Add("Controls", "Friends.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //最近访客
                mc = Regex.Match(path, @"(?<username>/\w+)/Guests.html$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Guests.aspx");
                    context.Items.Add("Controls", "Guests.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //博文列表
                mc = Regex.Match(path, @"(?<username>/\w+)/ArticleList_[\d]*_[\d]*\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/ArticleList.aspx");
                    context.Items.Add("Controls", "ArticleList.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 管理后台预览页
                mc = Regex.Match(path, @"(?<username>/\w+)/admin/PreviewPost.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/default.aspx");
                    context.Items.Add("Controls", "PreviewPost.ascx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 添加收藏页
                mc = Regex.Match(path, @"(?<username>/\w+)/AddToFavorite.aspx", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/AddToFavorite.aspx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 管理后台
                mc = Regex.Match(path, @"(?<username>/\w+)/admin/\w+\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath(context.Request.Path.Replace(mc.Groups["username"].Value, ""));
                    context.Items.Add("Path", path);
                    return true;
                }

                // 消息中心
                mc = Regex.Match(path, @"(?<username>/\w+)/MessageCenter/\w+\.(aspx|htm|html)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath(context.Request.Path.Replace(mc.Groups["username"].Value, ""));
                    context.Items.Add("Path", path);
                    return true;
                }

                // RSS订阅
                mc = Regex.Match(path, @"/rss.xml$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/rss.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //WidgetEdit
                mc = Regex.Match(path, @"(?<username>/\w+)/WidgetEdit.aspx$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/WidgetEdit.aspx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //StyleEdit
                mc = Regex.Match(path, @"(?<username>/\w+)/StyleEdit.aspx$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/StyleEdit.aspx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //评论列表
                mc = Regex.Match(path, @"(?<username>/\w+)/CommentList.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/UI/Controls/CommentList.do");
                    context.Items.Add("Path", path);
                    return true;
                }

                mc = Regex.Match(path, @"(?<username>/\w+)/PostComment.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/UI/Controls/PostComment.do");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发表评论用控件
                mc = Regex.Match(path, @"(?<username>/\w+)/Comment.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Comment.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //博客设置
                mc = Regex.Match(path, @"(?<username>/\w+)/Blog.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Blog.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发表文章
                mc = Regex.Match(path, @"(?<username>/\w+)/Entry.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Entry.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发送短消息
                mc = Regex.Match(path, @"(?<username>/\w+)/Scrip.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Scrip.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发送好友请求
                mc = Regex.Match(path, @"(?<username>/\w+)/Friend.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Friend.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发送访客
                mc = Regex.Match(path, @"(?<username>/\w+)/Guest.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Guest.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                // 新留言页面
                mc = Regex.Match(path, @"^/(\w|/)+/Message+[_]*[\d]*.(html|aspx|htm)$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/Message.aspx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发送留言
                mc = Regex.Match(path, @"(?<username>/\w+)/Message.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Message.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //发送收藏
                mc = Regex.Match(path, @"(?<username>/\w+)/Favorite.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Favorite.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                //分类管理页
                mc = Regex.Match(path, @"(?<username>/\w+)/Category.do$", RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    context.RewritePath("/DataManage/Category.ashx");
                    context.Items.Add("Path", path);
                    return true;
                }

                context.RewritePath(context.Request.Path);
                context.Items.Add("Path", path);
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion ReWriteUrl
    }
}
