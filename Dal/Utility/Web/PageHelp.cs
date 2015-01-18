using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.jk39.Utility
{
    public class PageHelp
    {
        /// <summary>
        /// 分页html
        /// </summary>
        /// <param name="curPage">当前页</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">当前页url</param>
        /// <param name="extendPage">显示页码个数</param>
        /// <returns></returns>
        public static string GetPageNumbers(int curPage, int RecordCount, int PageSize, string url, int extendPage, bool isRewrite)
        {
            int startPage = 1;
            int endPage = 1;

            #region 修正错误
            if (PageSize < 1)
                PageSize = 1;
            int countPage = RecordCount / PageSize + 1;
            if (RecordCount % PageSize == 0)
                countPage--;
            if (countPage < 1)
                countPage = 1;
            if (curPage < 1)
                curPage = 1;
            if (curPage > countPage)
                curPage = countPage;
            #endregion 修正错误

            string prePage = @"<a href=""{0}"">首页</a>
<a href=""{1}"">上一页</a>";
            string nextPage = @"<a href=""{0}"">下一页</a>
<a href=""{1}"">尾页</a>";

            if (isRewrite)
            {
                prePage = string.Format(prePage, string.Format(url, 1), string.Format(url, curPage - 1));
                nextPage = string.Format(nextPage, string.Format(url, curPage + 1), string.Format(url, countPage));
                if (curPage == 1)
                {
                    prePage = "";
                }
                if (curPage == countPage)
                {
                    nextPage = "";
                }
            }
            else
            {
                if (url.IndexOf("?") > 0)
                {
                    url = url + "&";
                }
                else
                {
                    url = url + "?";
                }
                prePage = string.Format(prePage, url + "pn=1", url + string.Format("pn={0}", curPage - 1));
                nextPage = string.Format(nextPage, url + string.Format("pn={0}", curPage + 1), url + string.Format("pn={0}", countPage));
                if (curPage == 1)
                {
                    prePage = "";
                }
                if (curPage == countPage)
                {
                    nextPage = "";
                }
            }

            if (countPage < 1)
                countPage = 1;
            if (extendPage < 3)
                extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                    }
                }
                else
                {
                    endPage = extendPage;
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;

            }
            StringBuilder s = new StringBuilder();
            s.AppendLine(prePage);
            s.AppendLine();
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                { 
                    s.Append(i); 
                }
                else
                {
                    s.Append("<a href=\"");
                    if (isRewrite)
                    {
                        s.Append(string.Format(url, i));
                    }
                    else
                    {
                        s.Append(url);
                        s.Append("pn=");
                        s.Append(i);
                    }
                    s.Append("\">");
                    s.Append(string.Format("[{0}]", i));
                    s.AppendLine("</a>");
                }
                s.AppendLine();
            }
            s.Append(nextPage);
            return s.ToString();

             
            //    <div id="pg">
            //    1
            //    <a href="/browse/79?lm=9&pn=25">[2]</a>
            //    <a href="/browse/79?lm=9&pn=50">[3]</a>
            //    <a href="/browse/79?lm=9&pn=75">[4]</a>
            //    <a href="/browse/79?lm=9&pn=100">[5]</a>
            //    <a href="/browse/79?lm=9&pn=125">[6]</a>
            //    <a href="/browse/79?lm=9&pn=150">[7]</a>
            //    <a href="/browse/79?lm=9&pn=175">[8]</a>
            //    <a href="/browse/79?lm=9&pn=200">[9]</a>
            //    <a href="/browse/79?lm=9&pn=225">[10]</a>
            //    <a href="/browse/79?lm=9&pn=25">下一页</a>
            //    <a href="/browse/79?lm=9&pn=2475">[尾页]</a>
            //    </div>
        }

    }
}
