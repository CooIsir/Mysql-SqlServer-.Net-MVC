using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace com.Utility
{

    /// <summary>
    /// CString 的摘要说明。
    /// </summary>
    public class CString
    {
        public CString() { }

        #region 常规字符串操作
        // 检查字符串是否为空
        public static bool IsEmpty(string str)
        {
            if (str == null || str == "")
                return true;
            else
                return false;
        }
        //检查字符串中是否包含非法字符
        public static bool CheckValidity(string s)
        {
            if (s == null)
                return false;
            if (s == "")
                return false;
            Match m = Regex.Match(s, "^\\w+$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            return m.Success;
        }
        public static DateTime ConvertToDate(string sDate)
        {
            DateTime dt = DateTime.Now;
            if (sDate == null)
                return dt;
            if (sDate == "")
                return dt;
            try
            {
                dt = DateTime.Parse(sDate);
            }
            catch { }
            return dt;
        }
        public static int CovnertToInt(string strNum, int defInt)
        {
            int iReturn = defInt;
            try
            {
                iReturn = int.Parse(strNum);
            }
            catch { }
            return iReturn;
        }
        // <summary> 
        /// 检测含有中文字符串的实际长度
        /// </summary> 
        /// <param name="str">字符串</param> 
        public static int GetLength(string str)
        {
            System.Text.ASCIIEncoding n = new System.Text.ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l 为字符串之实际长度 
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == 63) //判断是否为汉字或全脚符号 
                {
                    l++;
                }
                l++;
            }
            return l;
        }
        //截取长度,num是英文字母的总数，一个中文算两个英文
        public static string GetLetter(string str, int iNum, bool bAddDot)
        {
            string sContent = "";
            int iTmp = iNum;
            if (str == null)
                return sContent;
            else
                sContent = str;
            if (sContent.Length > 0)
            {
                if (iTmp > 0)
                {
                    if (sContent.Length * 2 > iTmp) //说明字符串的长度可能大于iNum,否则显示全部
                    {
                        char[] arrC;
                        if (sContent.Length >= iTmp) //防止因为中文的原因使ToCharArray溢出
                        {
                            arrC = str.ToCharArray(0, iTmp);
                        }
                        else
                        {
                            arrC = str.ToCharArray(0, sContent.Length);
                        }
                        int k = 0;
                        int i = 0;
                        int iLength = 0;
                        foreach (char ch in arrC)
                        {
                            iLength++;
                            if (char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter)
                            {
                                i += 2;
                            }
                            else
                            {
                                k = (int)ch;
                                if (k < 0)
                                {
                                    k = 65536;
                                }
                                if (k > 255)
                                {
                                    i += 2;
                                }
                                else
                                {
                                    if (k >= (int)'A' && k <= (int)'Z')
                                        i++;
                                    i++;
                                }
                            }
                            if (i >= iTmp)
                                break;
                        }
                        if (bAddDot)
                            sContent = sContent.Substring(0, iLength - 2) + "...";
                        else
                            sContent = sContent.Substring(0, iLength);
                    }
                }
            }
            return sContent;
        }
        //根据指定字符，截取相应字符串
        public static string GetStrByLast(string sOrg, string sLast)
        {
            int iLast = sOrg.LastIndexOf(sLast);
            if (iLast > 0)
                return sOrg.Substring(iLast + 1);
            else
                return sOrg;
        }
        public static string GetPreStrByLast(string sOrg, string sLast)
        {
            int iLast = sOrg.LastIndexOf(sLast);
            if (iLast > 0)
                return sOrg.Substring(0, iLast);
            else
                return sOrg;
        }
        public static bool IsVariableName(string s)
        {
            Regex re = new Regex(@"^[a-z][a-z0-9]*$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            return re.IsMatch(s);
        }
        #endregion  常规字符串操作

        #region HTML相关操作
        public static string ClearTag(string sHtml)
        {
            if (sHtml == "")
                return "";
            sHtml = CRegex.Replace(sHtml, CRegex.sIFrameReg, "", 0);
            sHtml = CRegex.Replace(sHtml, CRegex.sScriptReg, "", 0);
            sHtml = Regex.Replace(sHtml, @"<[^>]*>|&nbsp;", string.Empty, RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, @"\s\s", " ", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, @"\s\s", " ", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, @"\s\s", " ", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, "&ldquo;", "“", RegexOptions.IgnoreCase);
            sHtml = Regex.Replace(sHtml, "&rdquo;", "”", RegexOptions.IgnoreCase);
            return sHtml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sHtml"></param>
        /// <param name="bClear"></param>
        public static string ClearTag(string sHtml, bool bClear)
        {
            if (sHtml == "")
                return "";
            sHtml = CRegex.Replace(sHtml, CRegex.sIFrameReg, "", 0);
            sHtml = CRegex.Replace(sHtml, CRegex.sScriptReg, "", 0);
            sHtml = CRegex.Replace(sHtml, @"(<[^>\s]*\b(\w)+\b[^>]*>)|([\s]+)|(<>)|(&nbsp;)", "", 0);
            sHtml = sHtml.Replace("\"", "").Replace("<", "").Replace(">", "");
            return sHtml;
        }
        public static string ClearTag(string sHtml, string sRegex)
        {
            string sTemp = sHtml;
            Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return re.Replace(sHtml, "");
        }

        public static string ClearSpecial(string sInput)
        {
            sInput = sInput.Replace("\"", "'");
            return sInput;
        }

        public static string ConvertToJS(string sHtml)
        {
            StringBuilder sText = new StringBuilder();
            Regex re;
            re = new Regex(@"\r\n", RegexOptions.IgnoreCase);
            string[] strArray = re.Split(sHtml);
            foreach (string strLine in strArray)
            {
                sText.Append("document.writeln(\"" + strLine.Replace("\"", "\\\"") + "\");\r\n");
            }
            return sText.ToString();
        }
        public static string ReplaceNbsp(string str)
        {
            string sContent = str;
            if (sContent.Length > 0)
            {
                sContent = sContent.Replace(" ", "");
                sContent = sContent.Replace("&nbsp;", "");
                sContent = "&nbsp;&nbsp;&nbsp;&nbsp;" + sContent;
            }
            return sContent;
        }
        public static string StringToHtml(string str)
        {
            string sContent = str;
            if (sContent.Length > 0)
            {
                string csCr = "\r\n";
                sContent = sContent.Replace(csCr, "<br />");
            }
            return sContent;
        }
        public static string JsToHtml(string strJS)
        {
            string sReturn = strJS.Replace("document.writeln(\"", "");
            sReturn = sReturn.Replace("document.write(\"", "");
            sReturn = sReturn.Replace("document.write('", "");
            sReturn = CRegex.Replace(sReturn, @"(?<backslash>\\)[^\\]", "", "backslash");
            sReturn = sReturn.Replace(@"\\", @"\");
            sReturn = sReturn.Replace("/\\\\\\", "\\");
            sReturn = sReturn.Replace("/\\\\\\'", "\\'");
            sReturn = sReturn.Replace("/\\\\\\//", "\\/");
            sReturn = sReturn.Replace("\");", "");
            sReturn = sReturn.Replace("\")", "");
            sReturn = sReturn.Replace("');", "");
            return sReturn;
        }
        //截取长度并转换为HTML
        public static string AcquireAssignString(string str, int num)
        {
            string sContent = str;
            sContent = GetLetter(sContent, num, false);
            sContent = StringToHtml(sContent);
            return sContent;
        }
        //此方法与AcquireAssignString的功能已经一样，为了不报错，故保留此方法
        public static string TranslateToHtmlString(string str, int num)
        {
            string sContent = str;
            sContent = GetLetter(sContent, num, false);
            sContent = StringToHtml(sContent);
            return sContent;
        }
        public static string AddBlankAtForefront(string str)
        {
            string sContent = str;
            return sContent;
        }
        /// <summary>
        /// 获取需要的URL参数重新组装为参数串
        /// </summary>
        /// <param name="cQueryString">URL查询信息集合（Request.QueryString）</param>
        /// <param name="sKeys">需要重组的参数名数组</param>
        /// <returns></returns>
        public static string PassURLParameters(System.Collections.Specialized.NameValueCollection cQueryString, string[] sKeys)
        {
            if (sKeys == null)
                return "";
            string sPm = "";
            for (int i = 0; i < sKeys.Length; i++)
            {
                string temp = (string)cQueryString[sKeys[i]];
                if (temp != null)
                {
                    temp = "&" + sKeys[i] + "=" + temp;
                    sPm += temp;
                }
            }
            return sPm;
        }
        /// <summary>
        /// 获取一个Html标签里的所有属性键值对,属性值必须用双引号""包括
        /// Key的值默认转换成小写
        /// </summary>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetProperties(string sHtml)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Regex re = new Regex(@"\s+(?<Key>[^\s]+?)\s*=\s*""(?<Value>[\s\S]*?)(?<![\\])""");
            MatchCollection mc = re.Matches(sHtml);
            string sKey = null;
            foreach (Match m in mc)
            {
                sKey = m.Groups["Key"].Value.ToLower();
                if (!dic.ContainsKey(sKey))
                    dic.Add(m.Groups["Key"].Value.ToLower(), m.Groups["Value"].Value.Replace("\\\"", "\""));
            }
            return dic;
        }
        #endregion HTML相关操作

        #region 其他字符串操作
        /// <summary>
        /// 格式化字符串为 SQL 语句字段
        /// </summary>
        /// <param name="fldList"></param>
        /// <returns></returns>
        public static string GetSQLFildList(string fldList)
        {
            if (fldList == null)
                return "*";
            if (fldList.Trim() == "")
                return "*";
            if (fldList.Trim() == "*")
                return "*";
            //先去掉空格，[]符号
            string strTemp = fldList;
            strTemp = strTemp.Trim();
            strTemp = strTemp.Replace("[", "").Replace("]", "");
            //为防止使用保留字，给所有字段加上[]
            strTemp = "[" + strTemp + "]";
            strTemp = strTemp.Replace('，', ',');
            strTemp = strTemp.Replace(",", "],[");
            return strTemp;
        }
        /// <summary>
        /// 根据时间获得文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetPath(DateTime dt)
        {
            return dt.Year.ToString() + "/"
                   + dt.Month.ToString("00") + "/"
                   + dt.Day.ToString("00") + "/";
        }
        /// <summary>
        /// 根据时间获得文件名
        /// </summary>
        /// <returns></returns>
        public static string GetFileName(DateTime dt)
        {
            string sReturn = string.Format("{0:x}", dt.Ticks);
            sReturn = sReturn.Substring(4);
            return sReturn;
        }
        /// <summary>
        /// 将字符串按分隔符截取
        /// </summary>
        /// <param name="sCode">原字符串</param>
        /// <param name="cSeparator">分隔符</param>
        /// <param name="iSubCount">需要截取的子字符串所包含的子片断数</param>
        /// <param name="IsFromEnd">从前面还是后面取</param>
        /// <returns></returns>

        public static string ReassembleSubString(string sCode, char cSeparator, int iSubCount)
        {
            return ReassembleSubString(sCode, cSeparator, iSubCount, false);
        }

        public static string ReassembleSubString(string sCode, char cSeparator, int iSubCount, bool IsFromEnd)
        {
            string[] saSub = sCode.Split(cSeparator);
            if (iSubCount > saSub.Length)
            {
                return sCode;
            }
            else
            {
                if (IsFromEnd)
                {
                    int len = saSub.Length;
                    string temp = saSub[len - 1];
                    for (int i = 1; i < iSubCount; i++)
                        temp = saSub[len - 1 - i] + cSeparator.ToString() + temp;
                    return temp;
                }
                else
                {
                    string temp = saSub[0];
                    for (int i = 1; i < iSubCount; i++)
                        temp += cSeparator.ToString() + saSub[i];
                    return temp;
                }
            }
        }
        /// <summary>
        /// 将路径按分隔符截取
        /// </summary>
        /// <param name="sPath">原路径字符串</param>
        /// <param name="cSeparator">分隔符</param>
        /// <param name="iCutNum">截取数目</param>
        /// <returns></returns>
        public static string CutPath(string sPath, char cSeparator, int iCutNum)
        {
            string[] saSub = sPath.Split(cSeparator);
            int len = saSub.Length;
            if (len > 0)
            {
                string temp = saSub[len - iCutNum];
                for (int i = iCutNum - 1; i > 0; i--)
                    temp += cSeparator.ToString() + saSub[len - i];
                return temp;
            }
            else
                return sPath;
        }
        /// <summary> 
        /// 字符长度控制 中文 英文识别！ 
        /// 注：一个汉字作为2个字符长度处理 
        /// </summary> 
        /// <param name="str">要进行切割的字符串</param> 
        /// <param name="len">返回的长度（自动识别中英文）</param> 
        public static string CutString(string str, int len)
        {
            string newString = ""; int lengthi = Convert.ToByte(len.ToString());
            if (str.ToString() != "")
            {
                if (str.ToString().Length > lengthi)
                {
                    newString = str.ToString().Substring(0, lengthi) + "...";
                }
                else
                {
                    newString = str.ToString();
                }
            }
            return newString;
        }
        /// <summary> 
        /// 清除标签
        /// 截取字符串
        /// </summary> 
        /// <param name="str">要进行切割的字符串</param> 
        /// <param name="len">返回的长度（自动识别中英文）</param> 
        public static string CutString(string str, int len, bool bAddDot)
        {
            return CString.GetLetter(CString.ClearTag(str), len, bAddDot);
        }
        public static string SwitchStringSeparator(string sInputString, char OriginalSeparator, char NewSeparator)
        {
            string[] sSub = sInputString.Split(OriginalSeparator);
            if (sSub.Length > 1)
            {
                string sOutput = sSub[0];
                for (int i = 1; i < sSub.Length; i++)
                    sOutput += (sSub[i] == "") ? "" : NewSeparator.ToString() + sSub[i];
                return sOutput;
            }
            else
                return sInputString;
        }
        public static string SpecialCharTrim(string sInput, char c)
        {
            char[] arrC = new char[1];
            arrC[0] = c;
            return SpecialCharTrim(sInput, arrC);
        }
        public static string SpecialCharTrim(string sInput, char[] arrC)
        {
            for (int i = 0; i < arrC.Length; i++)
            {
                while (sInput.StartsWith(arrC[i].ToString()))
                    sInput = sInput.Substring(1);
                while (sInput.EndsWith(arrC[i].ToString()))
                    sInput = sInput.Remove(sInput.Length - 1, 1);
            }
            return sInput;
        }
        /// <summary>
        /// SQL AND条件添加
        /// </summary>
        /// <param name="sbWhere"></param>
        /// <param name="Condition"></param>
        public static void AddWhere(StringBuilder sbWhere, string Condition)
        {
            if (string.IsNullOrEmpty(Condition))
                return;
            if (sbWhere.Length == 0)
                sbWhere.Append(Condition);
            else
                sbWhere.Append(" and " + Condition);
        }
        #endregion 其他字符串操作


        #region:.包含文件解释
        public static string ReplaceInclude(string sContent, string sPath)
        {
            sContent = ReplaceIfTag(sContent, sPath);
            sContent = ReplaceIncTag(sContent, sPath);
            return sContent;
        }
        public static string ReplaceIfTag(string sContent, string sPath)
        {
            Regex regex = new Regex("<!--#if expr=\"\\$HTTP_HOST = '(.*?)'\" -->(.*?)<!--#else -->(.*?)<!--#endif -->", RegexOptions.Singleline);
            MatchCollection mc = regex.Matches(sContent);
            if (mc.Count == 0) return sContent;
            string sHost = string.Empty;
            string sExp = string.Empty;
            string incContent = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            {
                incContent = string.Empty;
                sHost = mc[i].Groups[1].Value;
                sExp = System.Web.HttpContext.Current.Request.Url.Host == sHost ? mc[i].Groups[2].Value : mc[i].Groups[3].Value;
                incContent = ReplaceIncTag(sExp, sPath);
                sContent = sContent.Replace(mc[i].Value, incContent);
            }
            return sContent;

        }
        public static string ReplaceIncTag(string sContent, string sPath)
        {
            Regex regex = new Regex("<!--#include virtual=\"(.*?)\".*?-->", RegexOptions.Singleline);
            MatchCollection mc = regex.Matches(sContent);
            if (mc.Count == 0) return sContent;
            string incPath = string.Empty;
            string incContent = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            {
                incContent = string.Empty;
                incPath = mc[i].Groups[1].Value;
                if (incPath.StartsWith("/"))
                {
                    incPath = System.Web.HttpContext.Current.Server.MapPath(incPath);
                }
                else if (incPath.StartsWith("../"))
                {
                    incPath = sPath.TrimEnd(new char[] { '\\' }) + @"\..\" + incPath.Replace("/", "\\");
                }
                else
                {
                    incPath = sPath.Substring(0, sPath.LastIndexOf(@"\") + 1) + incPath;
                }
                if (System.IO.File.Exists(incPath))
                {
                    incContent = CFile.Read(incPath);
                    incContent = ReplaceIncTag(incContent, incPath);
                }
                sContent = sContent.Replace(mc[i].Value, incContent);
            }
            return sContent;
        }
        #endregion 包含文件解释

        /// <summary>
        /// 替换除中文、字母、数字以外的字符 
        /// </summary>
        /// <param name="asString"></param>
        /// <returns></returns>
        public static string TrimCharacter(string asString)
        {
            string sResult = "";
            string sCharacterRegx = "[^a-zA-Z0-9\u4E00-\u9FFF]";
            Regex rgVal = new Regex(sCharacterRegx);
            sResult = rgVal.Replace(asString, "");
            return sResult;
        }
        public static string RemoveHtml(string content)
        {
            string newstr = FilterScript(content);
            string regexstr = @"<[^>]*>";
            return Regex.Replace(newstr, regexstr, string.Empty, RegexOptions.IgnoreCase).Replace("&nbsp;", "");
        }
        public static string FilterScript(string content)
        {
            if (content == null || content == "")
            {
                return content;
            }
            string regexstr = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";//@"<script.*</script>";
            content = Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<script([^>])*>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(content, "</script>", string.Empty, RegexOptions.IgnoreCase);
        }
    }
}