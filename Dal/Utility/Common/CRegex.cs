using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace com.Utility
{
    public class CRegex
    {
        public static bool IsQQ(string str_QQ)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_QQ, @"/^\d{5,11}$/");
        }
        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(string str_idcard)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");

        }
        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string str_postalcode)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");

        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str_Email"></param>
        /// <returns></returns>
        public static bool IsEmail(string str_Email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, @"\\w{1,}@\\w{1,}\\.\\w{1,}");
        }
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool  IsTelephone(string str_telephone)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^[1]+[3,5]+\d{9}");
        }

        public static readonly string sImgReg = "<img[^>]+src=\\s*(?:'(?<src>[\\w/\\./:-])'|\"(?<src>[\\w\\./:-]+)\"|(?<src>[\\w\\./:-]+))[^>]*>";
        public static readonly string sScriptReg = @"<script[\s\S]+?</script>";
        public static readonly string sIFrameReg = @"<iframe[\s\S]+?</iframe>";

        #region BaseMethod
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static bool IsMatch(string sInput, string sRegex)
        {
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Match mc = re.Match(sInput);
            return mc.Success;
        }
        /// <summary>
        /// 多个匹配内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="iGroupIndex">第几个分组, 从1开始, 0代表不分组</param>
        public static List<string> GetList(string sInput, string sRegex, int iGroupIndex)
        {
            List<string> list = new List<string>();
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            MatchCollection mcs = re.Matches(sInput);
            foreach (Match mc in mcs)
            {
                if (iGroupIndex > 0)
                    list.Add(mc.Groups[iGroupIndex].Value);
                else
                    list.Add(mc.Value);
            }
            return list;
        }
        /// <summary>
        /// 多个匹配内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="sGroupName">分组名, ""代表不分组</param>
        public static List<string> GetList(string sInput, string sRegex, string sGroupName)
        {
            List<string> list = new List<string>();
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            MatchCollection mcs = re.Matches(sInput);
            foreach (Match mc in mcs)
            {
                if (sGroupName != "")
                    list.Add(mc.Groups[sGroupName].Value);
                else
                    list.Add(mc.Value);
            }
            return list;
        }

        /// <summary>
        /// 多个匹配内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="sGroupName">分组名, ""代表不分组</param>
        public static List<string> GetList(string sInput, string sRegex, string sGroupName, string sContentUrlRule)
        {
            List<string> list = new List<string>();
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            MatchCollection mcs = re.Matches(sInput);
            foreach (Match mc in mcs)
            {
                if (sGroupName != "")
                {
                    if (IsPassUrlRule(mc.Groups[sGroupName].Value, sContentUrlRule))
                        list.Add(mc.Groups[sGroupName].Value);
                    else
                        continue;
                }
                else
                {
                    if (IsPassUrlRule(mc.Value, sContentUrlRule))
                        list.Add(mc.Value);
                    else
                        continue;
                }
            }
            return list;
        }

        /// <summary>
        /// 判断是否通过链接规则
        /// </summary>
        /// <param name="sValue">要判断的链接</param>
        /// <param name="sRule">链接规则表达式字符串,""认为没有规则</param>
        /// <returns>匹配结果</returns>
        private static bool IsPassUrlRule(string sValue, string sRule)
        {
            if (sRule != "" && sRule != null)
            {
                Regex re = new Regex(sRule, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                return re.IsMatch(sValue);
            }
            return true;
        }


        /// <summary>
        /// 单个匹配内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="iGroupIndex">分组序号, 从1开始, 0不分组</param>
        public static string GetText(string sInput, string sRegex, int iGroupIndex)
        {
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Match mc = re.Match(sInput);
            if (mc.Success)
            {
                if (iGroupIndex > 0)
                    return mc.Groups[iGroupIndex].Value;
                else
                    return mc.Value;
            }
            else
                return "";
        }
        /// <summary>
        /// 单个匹配内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="sGroupName">分组名, ""代表不分组</param>
        public static string GetText(string sInput, string sRegex, string sGroupName)
        {
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Match mc = re.Match(sInput);
            if (mc.Success)
            {
                if (sGroupName != "")
                    return mc.Groups[sGroupName].Value;
                else
                    return mc.Value;
            }
            else
                return "";
        }
        /// <summary>
        /// 替换指定内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="sReplace">替换值</param>
        /// <param name="iGroupIndex">分组序号, 0代表不分组</param>
        public static string Replace(string sInput, string sRegex, string sReplace, int iGroupIndex)
        {
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            MatchCollection mcs = re.Matches(sInput);
            foreach (Match mc in mcs)
            {
                if (iGroupIndex > 0)
                    sInput = sInput.Replace(mc.Groups[iGroupIndex].Value, sReplace);
                else
                    sInput = sInput.Replace(mc.Value, sReplace);
            }
            return sInput;
        }
        /// <summary>
        /// 替换指定内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="sReplace">替换值</param>
        /// <param name="sGroupName">分组名, "" 代表不分组</param>
        public static string Replace(string sInput, string sRegex, string sReplace, string sGroupName)
        {
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            MatchCollection mcs = re.Matches(sInput);
            foreach (Match mc in mcs)
            {
                if (sGroupName != "")
                    sInput = sInput.Replace(mc.Groups[sGroupName].Value, sReplace);
                else
                    sInput = sInput.Replace(mc.Value, sReplace);
            }
            return sInput;
        }
        /// <summary>
        /// 分割
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        /// <param name="iStrLen">最小保留字符串长度</param>
        public static List<string> Split(string sInput, string sRegex, int iStrLen)
        {
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            string[] sArray = re.Split(sInput);
            List<string> list = new List<string>();
            list.Clear();
            foreach (string s in sArray)
            {
                if (s.Trim().Length < iStrLen)
                    continue;
                list.Add(s.Trim());
            }
            return list;
        }
        #endregion BaseMethod

        #region 获得特定内容
        /// <summary>
        /// 多个链接
        /// </summary>
        /// <param name="sInput">输入内容</param>
        public static List<string> GetLinks(string sInput)
        {
            return GetList(sInput, @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>", "href");
        }
        /// <summary>
        /// 单个链接
        /// </summary>
        /// <param name="sInput">输入内容</param>
        public static string GetLink(string sInput)
        {
            return GetText(sInput, @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>", "href");
        }
        /// <summary>
        /// 图片标签
        /// </summary>
        /// <param name="sInput">输入内容</param>
        public static List<string> GetImgTag(string sInput)
        {
            return GetList(sInput, "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", "");
        }
        /// <summary>
        /// 图片地址
        /// </summary>
        /// <param name="sInput">输入内容</param>
        public static string GetImgSrc(string sInput)
        {
            return GetText(sInput, "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", "src");
        }
        /// <summary>
        /// 根据URL获得域名
        /// </summary>
        /// <param name="sUrl">输入内容</param>
        public static string GetDomain(string sInput)
        {
            return GetText(sInput, @"http(s)?://([\w-]+\.)+(\w){2,}", 0);
        }
        /// <summary>
        /// 根据URL获得Host,不带协议名
        /// </summary>
        /// <param name="sUrl">输入内容</param>
        public static string GetHost(string sInput)
        {
            return GetText(sInput, @"(http(s)?://)?(?<Host>([\w-]+\.)+(\w){2,})", "Host");
        }

        /// <summary>
        /// 域名验证
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static bool IsDomain(string sInput)
        {
            string rgx = @"^([\w-]+\.)+(\w){2,}$";
            return IsMatch(sInput, rgx);
        }
        #endregion 获得特定内容

        #region 根据表达式，获得文章内容
        /// <summary>
        /// 文章标题
        /// </summary>
        /// <param name="sInput">输入内容</param>
        public static string GetTitle(string sInput, string sRegex)
        {
            string sTitle = GetText(sInput, sRegex, "Title");
            sTitle = CString.ClearTag(sTitle);
            if (sTitle.Length > 99)
            {
                sTitle = sTitle.Substring(0, 99);
            }
            return sTitle;
        }
        /// <summary>
        /// 网页内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static string GetHtml(string sInput)
        {
            return CRegex.Replace(sInput, @"(?<Head>[^<]+)<", "", "Head");
        }

        public static string GetBody(string sInput)
        {
            return GetText(sInput, @"<Body[^>]*>(?<Body>[\s\S]{10,})</body>", "Body");
        }
        /// <summary>
        /// 网页Body内容
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static string GetBody(string sInput, string sRegex)
        {
            return GetText(sInput, sRegex, "Body");
        }
        /// <summary>
        /// 文章来源
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static string GetSource(string sInput, string sRegex)
        {
            string sSource = GetText(sInput, sRegex, "Source");
            sSource = CString.ClearTag(sSource);
            if (sSource.Length > 99)
                sSource = sSource.Substring(0, 99);
            return sSource;
        }
        /// <summary>
        /// 作者名
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static string GetAuthor(string sInput, string sRegex)
        {
            string sAuthor = GetText(sInput, sRegex, "Author");
            sAuthor = CString.ClearTag(sAuthor);
            if (sAuthor.Length > 99)
                sAuthor = sAuthor.Substring(0, 99);
            return sAuthor;
        }
        /// <summary>
        /// 分页链接地址
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static List<string> GetPageLinks(string sInput, string sRegex)
        {
            return GetList(sInput, sRegex, "href");
        }
        /// <summary>
        /// 根据相对路径得到绝对路径
        /// </summary>
        /// <param name="sUrl">输入内容</param>
        /// <param name="sInput">原始网站地址</param>
        /// <param name="sRelativeUrl">相对链接地址</param>
        public static string GetUrl(string sInput, string sRelativeUrl)
        {
            string sReturnUrl = "";
            string sUrl1 = sInput.Trim();
            if (sRelativeUrl.ToLower().StartsWith("http") || sRelativeUrl.ToLower().StartsWith("https"))
            {
                sReturnUrl = sRelativeUrl.Trim();
            }
            else if (sRelativeUrl.StartsWith("/"))
            {
                sReturnUrl = GetDomain(sInput) + sRelativeUrl;
            }
            else if (sRelativeUrl.StartsWith("../"))
            {
                while (sRelativeUrl.IndexOf("../") >= 0)
                {
                    sUrl1 = CString.GetPreStrByLast(sUrl1, "/");
                    sRelativeUrl = sRelativeUrl.Substring(3);
                }
                sReturnUrl = sUrl1 + "/" + sRelativeUrl.Trim();
            }
            else if (sRelativeUrl.Trim() != "")
            {
                sReturnUrl = CString.GetPreStrByLast(sInput, "/") + "/" + sRelativeUrl;
            }
            else
            {
                sRelativeUrl = sInput;
            }
            return sReturnUrl;
        }
        /// <summary>
        /// 关键字
        /// </summary>
        /// <param name="sInput">输入内容</param>
        public static string GetKeyWord(string sInput)
        {
            List<string> list = Split(sInput, "(,|，|\\+|＋|。|;|；|：|:|“)|”|、|_|\\(|（|\\)|）", 2);
            List<string> listReturn = new List<string>();
            Regex re;
            foreach (string str in list)
            {
                re = new Regex(@"[a-zA-z]+", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                MatchCollection mcs = re.Matches(str);
                string sTemp = str;
                foreach (Match mc in mcs)
                {
                    if (mc.Value.ToString().Length > 2)
                        listReturn.Add(mc.Value.ToString());
                    sTemp = sTemp.Replace(mc.Value.ToString(), ",");
                }
                re = new Regex(@",{1}", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                mcs = re.Matches(sTemp);
                foreach (string s in re.Split(sTemp))
                {
                    if (s.Trim().Length <= 2)
                        continue;
                    listReturn.Add(s);
                }
            }
            string sReturn = "";
            for (int i = 0; i < listReturn.Count - 1; i++)
            {
                for (int j = i + 1; j < listReturn.Count; j++)
                {
                    if (listReturn[i] == listReturn[j])
                    {
                        listReturn[j] = "";
                    }
                }
            }
            foreach (string str in listReturn)
            {
                if (str.Length > 2)
                    sReturn += str + ",";
            }
            if (sReturn.Length > 0)
                sReturn = sReturn.Substring(0, sReturn.Length - 1);
            else
                sReturn = sInput;
            if (sReturn.Length > 99)
                sReturn = sReturn.Substring(0, 99);
            return sReturn;
        }
        /// <summary>
        /// 发布日期
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sRegex">表达式字符串</param>
        public static DateTime GetCreateDate(string sInput, string sRegex)
        {
            DateTime dt = System.DateTime.Now;
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Match mc = re.Match(sInput);
            if (mc.Success)
            {
                try
                {
                    int iYear = int.Parse(mc.Groups["Year"].Value);
                    int iMonth = int.Parse(mc.Groups["Month"].Value);
                    int iDay = int.Parse(mc.Groups["Day"].Value);
                    int iHour = dt.Hour;
                    int iMinute = dt.Minute;

                    string sHour = mc.Groups["Hour"].Value;
                    string sMintue = mc.Groups["Mintue"].Value;

                    if (sHour != "")
                        iHour = int.Parse(sHour);
                    if (sMintue != "")
                        iMinute = int.Parse(sMintue);

                    dt = new DateTime(iYear, iMonth, iDay, iHour, iMinute, 0);
                }
                catch { }
            }
            return dt;
        }

        public static string GetContent(string sOriContent, string sOtherRemoveReg, string sPageUrl)
        {
            string sFormartted = sOriContent;
            sFormartted = Regex.Replace(sFormartted, @"<script[\s\S]*?</script>", "", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            sFormartted = Regex.Replace(sFormartted, @"<iframe[^>]*>[\s\S]*?</iframe>", "", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            string[] sOtherReg = sOtherRemoveReg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string sRemoveReg in sOtherReg)
            {
                sFormartted = CRegex.Replace(sFormartted, sRemoveReg, "", 0);
            }
            Regex re = new Regex("<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            MatchCollection mcs = re.Matches(sFormartted);
            string sOriStr = "";
            string sReplaceStr = "";
            foreach (Match mc in mcs)
            {
                sOriStr = mc.Value;
                sReplaceStr = sOriStr.Replace(mc.Groups["src"].Value, CRegex.GetUrl(sPageUrl, mc.Groups["src"].Value));
                sFormartted = sFormartted.Replace(sOriStr, sReplaceStr);
            }
            re = new Regex(@"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            mcs = re.Matches(sFormartted);
            sOriStr = "";
            sReplaceStr = "";
            foreach (Match mc in mcs)
            {
                sOriStr = mc.Value;
                sReplaceStr = CRegex.Replace(sOriStr, @"<[^>]*\ba\b[^>]*>", "", 0);
                sFormartted = sFormartted.Replace(sOriStr, sReplaceStr);
            }
            return sFormartted;
        }
        #endregion 根据表达式，获得文章内容

        #region  常用校验表达式
        public static bool CheckUrl(string sInput)
        {
            string sRegex = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            Match mc = Regex.Match(sInput, sRegex, RegexOptions.IgnoreCase);
            return (mc.Success);
        }
        #endregion   常用校验表达式

        #region 获得特定内容
        /// <summary>
        /// 多个链接
        /// </summary>
        /// <param name="sInput">输入内容</param>
        //public static List<string> GetLinks(string sInput)
        //{
        //    return GetList(sInput, @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>", "href");
        //}
        /// <summary>
        /// 多个链接
        /// </summary>
        /// <param name="sInput">输入内容</param>
        /// <param name="sContentUrlRule">链接规则</param>
        public static List<string> GetLinks(string sInput, string sContentUrlRule)
        {
            return GetList(sInput, @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>", "href", sContentUrlRule);
        }
        /// <summary>
        /// 多个链接(带链接内容)
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="sContentUrlRule"></param>
        /// <returns></returns>
        //public static List<KeyValuePair<string, string>> GetLinksAndTitle(string sInput, string sContentUrlRule)
        //{
        //    return GetKeyValueList(sInput, @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>(?<LinkTitle>[\s\S]+?)</a>", sContentUrlRule);
        //}
        /// <summary>
        /// 单个链接
        /// </summary>
        /// <param name="sInput">输入内容</param>
        //public static string GetLink(string sInput)
        //{
        //    return GetText(sInput, @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>", "href");
        //}
        /// <summary>
        /// 图片标签
        /// </summary>
        /// <param name="sInput">输入内容</param>
        //public static List<string> GetImgTag(string sInput)
        //{
        //    return GetList(sInput, "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", "");
        //}
        /// <summary>
        /// 图片地址
        /// </summary>
        /// <param name="sInput">输入内容</param>
        //public static string GetImgSrc(string sInput)
        //{
        //    return GetText(sInput, "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>", "src");
        //}
        /// <summary>
        /// 根据URL获得域名
        /// </summary>
        /// <param name="sUrl">输入内容</param>
        //public static string GetDomain(string sInput)
        //{
        //    return GetText(sInput, @"http(s)?://([\w-]+\.)+(\w){2,}", 0);
        //}

        public static int GetPostID(string sInput)
        {
            return int.Parse(GetText(sInput, @"http://community.39.net/Topics/[\w]+/(?<ID>[\d]+).xml", "ID"));
        }
        #endregion 获得特定内容



        //除掉保存的字符串内容的所有html格式，也就是只取纯文本
        public static string FilterHTML(string source)
        {
            string result;            //remove line breaks,tabs
            result = source.Replace("\r", " ");
            result = result.Replace("\n", " ");
            result = result.Replace("\t", " ");

            //remove the header
            result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);

            result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);

            //remove all styles
            result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase); //clearing attributes
            result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);

            //insert tabs in spaces of <td> tags
            result = Regex.Replace(result, @"<( )*td([^>])*>", " ", RegexOptions.IgnoreCase);

            //insert line breaks in places of <br> and <li> tags
            result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);

            //insert line paragraphs in places of <tr> and <p> tags
            result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);

            //remove anything thats enclosed inside < >
            result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);

            //replace special characters:
            result = Regex.Replace(result, @"&", "&", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @" ", " ", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<", "<", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @">", ">", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);

            //remove extra line breaks and tabs
            result = Regex.Replace(result, @" ( )+", " ");
            result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r");
            result = Regex.Replace(result, @"(\r\r)+", "\r\n");

            //remove blank
            result = Regex.Replace(result, @"\s", "");

            return result;
        }
    }
}
