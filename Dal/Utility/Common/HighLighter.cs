using System.Collections.Generic;

namespace com.Utility
{
    public class HighLighter
    {
        /// <summary>
        /// 高亮某段文字里面的关键字
        /// </summary>
        /// <param name="keyWords">要高亮显示的关键字数组集合</param>
        /// <param name="Content">原文内容</param>
        /// <param name="cssClass">高亮部分的CSS样式类名</param>
        /// <returns>经高亮处理的文本</returns>
        public static string HighLight(string[] keyWords, string Content, string cssClass)
        {
            foreach (string keyword in keyWords)
            {
                Content = Content.Replace(keyword, string.Format("<span class=\"{0}\">{1}</span>", cssClass, keyword));
            }
            return Content;
        }

        /// <summary>
        /// 高亮某段文字里面的关键字
        /// </summary>
        /// <param name="keyWords">要高亮显示的关键字数组集合</param>
        /// <param name="Content">原文内容</param>
        /// <param name="cssClass">高亮部分的CSS样式类名</param>
        /// <returns>经高亮处理的文本</returns>
        public static string HighLight(List<string> keyWords, string Content, string cssClass)
        {
            foreach (string keyword in keyWords)
            {
                Content = Content.Replace(keyword, string.Format("<span class=\"{0}\">{1}</span>", cssClass, keyword));
            }
            return Content;
        }
    }
}