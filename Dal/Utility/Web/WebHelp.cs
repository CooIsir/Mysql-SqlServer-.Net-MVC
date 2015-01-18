using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Net;

namespace com.Utility
{
    public class WebHelp
    {  
        #region 绑定DropDownList

        public static void BindDropDownList(DataTable dt, DropDownList ddl, string sValueField, string sTextField)
        {
            BindDropDownList(dt, ddl, sValueField, sTextField, 0);
        }

        public static void BindDropDownList(DataTable dt, DropDownList ddl, string sValueField, string sTextField, int iSelectedIndex)
        {
            BindDropDownList(dt.DefaultView, ddl, sValueField, sTextField, iSelectedIndex);
        }

        public static void BindDropDownList(DataTable dt, DropDownList ddl, string sValueField, string sTextField, string sPreText)
        {
            BindDropDownList(dt.DefaultView, ddl, sValueField, sTextField, sPreText);
        }

        // 绑定DataView
        public static void BindDropDownList(DataView dv, DropDownList ddl, string sValueField, string sTextField)
        {
            BindDropDownList(dv, ddl, sValueField, sTextField, 0);
        }

        public static void BindDropDownList(DataView dv, DropDownList ddl, string sValueField, string sTextField, int iSelectedIndex)
        {
            BindDropDownList(dv, ddl, sValueField, sTextField, "");
            if (ddl.Items.Count > 0)
            {
                try
                {
                    ddl.SelectedIndex = iSelectedIndex;
                }
                catch
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        public static void BindDropDownList(DataView dv, DropDownList ddl, string sValueField, string sTextField, string sPreText)
        {
            ddl.Items.Clear();
            if (ddl == null || dv == null)
                return;
            if (!CString.IsEmpty(sPreText))
                ddl.Items.Add(new ListItem(sPreText, "0"));
            foreach (DataRowView drv in dv)
            {
                ddl.Items.Add(new ListItem(drv[sTextField].ToString(), drv[sValueField].ToString()));
            }
            if (ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;
        }

        #endregion

        #region 绑定DropDownList

        public static void BindDropDownList(List<CustomListItem> listItem, DropDownList ddl)
        {
            BindDropDownList(listItem, ddl, 0);
        }

        public static void BindDropDownList(List<CustomListItem> listItem, DropDownList ddl, int iSelectedIndex)
        {
            BindDropDownList(listItem, ddl, "");
            if (ddl.Items.Count > 0)
            {
                try
                {
                    ddl.SelectedIndex = iSelectedIndex;
                }
                catch
                {
                    ddl.SelectedIndex = 0;
                }
            }
        }

        public static void BindDropDownList(List<CustomListItem> listItem, DropDownList ddl, string sPreText)
        {
            ddl.Items.Clear();
            if (!string.IsNullOrEmpty(sPreText))
                ddl.Items.Add(new ListItem(sPreText, "0"));
            if (ddl == null || listItem == null)
                return;
            if (listItem.Count == 0)
                return;

            foreach (CustomListItem mItem in listItem)
            {
                ddl.Items.Add(new ListItem(mItem.Text, mItem.Value));
            }
            if (ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;
        }

        #endregion

        #region 绑定ListBox

        public static void BindListBox(List<CustomListItem> list, ListBox listBox)
        {
            listBox.Items.Clear();
            if (list == null || listBox == null)
                return;
            if (list.Count == 0)
                return;
            foreach (CustomListItem mItem in list)
            {
                listBox.Items.Add(new ListItem(mItem.Text, mItem.Value));
            }
        }

        #endregion 绑定ListBox

        #region 绑定CheckBoxList
        public static void BindCheckBoxList(List<CustomListItem> listItem, CheckBoxList cbList)
        {
            if (cbList == null)
                return;
            cbList.Items.Clear();
            if (listItem == null || listItem.Count == 0)
                return;
            foreach (CustomListItem mItem in listItem)
            {
                cbList.Items.Add(new ListItem(mItem.Text, mItem.Value));
            }
        }
        #endregion 绑定CheckBoxList

        #region 加载树形控件

        public static void LoadTree(TreeView trv, DataTable dt, string sValueField, string sTextField, int iParentID)
        {
            trv.Nodes.Clear();
            foreach (DataRowView drv in dt.DefaultView)
            {
                if (drv["ParentID"].ToString() == iParentID.ToString())
                {
                    TreeNode RootNode = new TreeNode();
                    RootNode.Value = drv[sValueField].ToString();
                    RootNode.Text = drv[sTextField].ToString();
                    trv.Nodes.Add(RootNode);
                    LoadChildNode(dt, RootNode, sValueField, sTextField);
                }
            }
        }

        private static void LoadChildNode(DataTable dt, TreeNode ParentNode, string sValueField, string sTextField)
        {
            foreach (DataRowView drv in dt.DefaultView)
            {
                if (drv["ParentID"].ToString() == ParentNode.Value)
                {
                    TreeNode ChildNode = new TreeNode();
                    ChildNode.Value = drv[sValueField].ToString();
                    ChildNode.Text = drv[sTextField].ToString();
                    ParentNode.ChildNodes.Add(ChildNode);
                    LoadChildNode(dt, ChildNode, sValueField, sTextField);
                }
            }
        }

        public static void LoadTree(TreeView trv, List<CustomTreeNode> listTreeNode, string strParentID)
        {
            trv.Nodes.Clear();
            List<CustomTreeNode> listChild = FindChild(listTreeNode, strParentID);

            foreach (CustomTreeNode mNode in listChild)
            {
                TreeNode RootNode = new TreeNode();
                RootNode.Value = mNode.Value;
                RootNode.Text = mNode.Text;
                trv.Nodes.Add(RootNode);
                LoadChildNode(listTreeNode, RootNode);
            }
        }

        public static void LoadNode(TreeNode node, List<CustomTreeNode> listTreeNode, string strParentID)
        {
            node.ChildNodes.Clear();
            List<CustomTreeNode> listChild = FindChild(listTreeNode, strParentID);

            foreach (CustomTreeNode mNode in listChild)
            {
                TreeNode newNode = new TreeNode();
                newNode.Value = mNode.Value;
                newNode.Text = mNode.Text;
                node.ChildNodes.Add(newNode);
                LoadChildNode(listTreeNode, newNode);
            }
        }

        private static void LoadChildNode(List<CustomTreeNode> listTreeNode, TreeNode ParentNode)
        {
            List<CustomTreeNode> listChild = FindChild(listTreeNode, ParentNode.Value);
            foreach (CustomTreeNode mNode in listChild)
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Value = mNode.Value;
                ChildNode.Text = mNode.Text;
                ParentNode.ChildNodes.Add(ChildNode);
                LoadChildNode(listTreeNode, ChildNode);
            }
        }

        private static List<CustomTreeNode> FindChild(List<CustomTreeNode> listNode, string strParentID)
        {
            List<CustomTreeNode> list = new List<CustomTreeNode>();
            if (listNode == null)
                return list;
            if (listNode.Count == 0)
                return list;
            foreach (CustomTreeNode mNode in listNode)
            {
                if (mNode.ParentID == strParentID)
                    list.Add(mNode);
            }
            return list;
        }

        #endregion 加载树形控件

        #region  获得时间差

        public static int GetTimeDelay(DateTime dtStart, DateTime dtEnd)
        {
            int iReturn = 0;
            iReturn += (dtEnd.Hour - dtStart.Hour)*60*60*1000;
            iReturn += (dtEnd.Minute - dtStart.Minute)*60*1000;
            iReturn += (dtEnd.Second - dtStart.Second)*1000;
            iReturn += dtEnd.Millisecond - dtStart.Millisecond;
            return iReturn;
        }

        #endregion  获得时间差

        #region 分页器
        /// <summary>
        /// 分页html
        /// </summary>
        /// <param name="curPage">当前页</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">当前页url</param>
        /// <param name="extendPage">显示页码个数</param>
        /// <returns></returns>
        public static string GetPageNumbers(int curPage, int RecordCount,int PageSize, string url, int extendPage, bool isRewrite)
        {
            int startPage = 1;
            int endPage = 1;

            #region 修正错误
            if (PageSize < 1)
                PageSize = 1;
            int countPage = RecordCount / PageSize+1;
            if (RecordCount % PageSize == 0)
                countPage--;
            if (countPage < 1)
                countPage = 1;
            if (curPage < 1)
                curPage = 1;
            if (curPage > countPage)
                curPage = countPage;
            #endregion 修正错误

            string prePage = @"<a href=""{0}"" class=""first"">首页</a>&nbsp;<a href=""{1}"" class=""prev"">上一页</a>&nbsp;";
            string nextPage = @"<a href=""{0}"" class=""next"">下一页</a>&nbsp;<a href=""{1}"" class=""last"">尾页</a>";

            if (isRewrite)
            { 
                prePage = string.Format(prePage, string.Format(url, 1),  string.Format(url, curPage - 1));
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
                prePage = string.Format(prePage, url +"pn=1", url + string.Format("pn={0}", curPage - 1));
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
            StringBuilder s = new StringBuilder("");
            s.Append(prePage); 
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append(@"<span style=""color:Red"">");
                    s.Append(i);
                    s.Append("</span>&nbsp;");
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
                    s.Append(i);
                    s.Append("</a>");
                    s.Append("&nbsp;");
                }
            } 
            s.Append(nextPage);
            return s.ToString();
              //<a href="" class="first">首页</a> <a href="" class="prev">上一页</a> <a href="">1</a> <a href="">2</a> <a href="">3</a> <a href="">4</a> <a href="">5</a> <a href="">6</a> <a href="" class="next">下一页</a> <a href="" class="last">尾页</a>
        }
        #endregion

        /// <summary>
        /// MD5加密
        /// 2008-02-21 创建
        /// </summary>
        /// <param name="clearString">明文</param>
        /// <returns></returns>
        public static string ToMd5(string clearString)
        {
            Byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearString);
            string hashedPwd = BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes));
            return hashedPwd;
        }

        #region 查找树中的指定节点
        public static TreeNode FindNodeByNodeValue(TreeView tv, string nodeValue)
        {
            TreeNode n = null;
            foreach (TreeNode node in tv.Nodes)
            {
                if (node.Value.Equals(nodeValue))
                {
                    n = node;
                    break;
                }
                else
                {
                    n = FindNodeByNodeValue(node, nodeValue);
                    if (n != null)
                    {
                        break;
                    }
                }
            }
            return n;
        }

        private static TreeNode FindNodeByNodeValue(TreeNode parentNode, string nodeValue)
        {
            TreeNode n = null;
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Value.Equals(nodeValue))
                {
                    n = node;
                    break;
                }
                else
                {
                    n = FindNodeByNodeValue(node, nodeValue);
                    if (n != null)
                    {
                        break;
                    }
                }
            }
            return n;
        }

        #endregion

        #region 获得IP
        public static string GetIP()
        {
            string[] IP_Ary;
            string strIP, strIP_list;
            strIP_list = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIP_list != null && strIP_list != "")
            {
                strIP_list = strIP_list.Replace("'", "");
                if (strIP_list.IndexOf(",") >= 0)
                {
                    IP_Ary = strIP_list.Split(',');
                    strIP = IP_Ary[0];
                }
                else
                {
                    strIP = strIP_list;
                }
            }
            else
            {
                strIP = "";
            }
            if (strIP == "")
            {
                strIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                strIP = strIP.Replace("'", "");
            }
            return strIP;
        }

        /// <summary>
        /// 获得服务器本机Ip
        /// </summary>
        /// <returns></returns>
        public UInt32 GetServerIpAddresses()
        {
            //string cLocalComputerName = null;
            //cLocalComputerName = Dns.GetHostName(); //本地计算机的DNS主机名

            //string cIPAddresses = ""; //本地计算机的IP地址  
            //try
            //{
            //    System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
            //    IPHostEntry heLocalComputer = Dns.GetHostEntry(cLocalComputerName);
            //    foreach (IPAddress curAdd in heLocalComputer.AddressList)
            //    {
            //        Byte[] bytes = curAdd.GetAddressBytes();
            //        for (int i = 0; i < bytes.Length; i++)
            //        {
            //            cIPAddresses += bytes[i].ToString() + ".";
            //        }
            //    }
            //    cIPAddresses = cIPAddresses.Substring(0, cIPAddresses.Length - 1);
            //}
            //catch (Exception e)
            //{
            //    string cErrMsg = e.ToString();
            //    cIPAddresses = "";
            //}
            //return cIPAddresses; 
            return BitConverter.ToUInt32(IPAddress.Parse(System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString ()).GetAddressBytes(), 0);
        }

        #endregion 获得IP
    }
}