using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.IO;

namespace com.Utility
{
    public class jsHelper : System.Web.UI.Page
    {
        #region 打开窗口
        public static void openFullWindow(Page thisPage, string sURL)
        {
            thisPage.Response.Write("<script language=javascript> var x =screen.availWidth -10;</script>");
            thisPage.Response.Write("<script language=javascript> var y =screen.availHeight - 50;</script>");
            thisPage.Response.Write("<script language=javascript> window.open('" + sURL + "','NEW','resizable=yes,status=yes,toolbar=no,menubar=no,location=no,height='+ y +',width='+ x +',top=0,left=0,fullscreen=0');</script>");
            //thisPage.Response.Write("<script language=javascript> window.open('Includes/WebMain.aspx','Main_WIN','resizable=yes,status=yes,toolbar=no,menubar=no,location=no,height=' + y + ',width=' + x + ',top=0,left=0,fullscreen=0');</script>");
            //height=100, width=400, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no
        }
        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="self"></param>
        public static void OpenWindow(Page page, string url, string self)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript'>window.open('" + url + "',target ='" + self + "');<" + "/script>");
        }
        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="self"></param>
        public static void OpenWindow(Page thisPage, string sUrl, int iWidth, int iHeight)
        {
            //thisPage.Response.Write("<script language=javascript> var x =screen.availWidth -10;</script>");
            //thisPage.Response.Write("<script language=javascript> var y =screen.availHeight - 50;</script>");
            thisPage.Response.Write("<script language=javascript> var x1 =(screen.availWidth + 20 - " + iWidth + ")/2;</script>");
            thisPage.Response.Write("<script language=javascript> var y1 =(screen.availHeight - " + iHeight + ")/2;</script>");
            thisPage.Response.Write("<script language=javascript> var x =" + iWidth + ";</script>");
            thisPage.Response.Write("<script language=javascript> var y =" + iHeight + ";</script>");
            thisPage.Response.Write("<script language=javascript> window.open('" + sUrl + "','NEW','resizable=yes,status=yes,toolbar=no,menubar=no,location=no,height='+ y +',width='+ x +',top='+ y1 +',left='+ y1 +',fullscreen=0');</script>");
        }
        public static void ShowModalDialog(Page thisPage, string sUrl, int Width, int Height)
        {
            string jsText = string.Format("<script language='javascript' type='text/javascript'>window.showModalDialog('{0}','', 'dialogWidth={1}px;dialogHeight={2}px;top=0;left=0;toolbar=no;menubar=no;scrollbars=no;resizable=no;location=no;status=no'); </script>", sUrl, Width, Height);
            thisPage.ClientScript.RegisterStartupScript(typeof(String), "", jsText);
        }
        public static void OpenModalDialogWindow(Page thisPage, string sURL, int iHeight, int iWidth, bool bResize, bool bStatus)
        {
            string sFeatures = "dialogHeight: " + iHeight.ToString() + "px; dialogWidth: " + iWidth.ToString() + "px;  resizable: " + (bResize ? "yes" : "no") + "; status: " + (bStatus ? "yes" : "no") + ";";
            thisPage.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript'>window.showModalDialog('" + sURL + "','','" + sFeatures + "')</script>");
        }
        public static void OpenModalDialogWindow(Page thisPage, string sURL, int iHeight, int iWidth, bool bResize, bool bStatus, string sHiddenID)
        {
            string sFeatures = "dialogHeight: " + iHeight.ToString() + "px; dialogWidth: " + iWidth.ToString() + "px;  resizable: " + (bResize?"yes":"no") + "; status: " + (bStatus?"yes":"no") + ";";
            thisPage.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript'>document.getElementById('" + sHiddenID + "').value=window.showModalDialog('" + sURL + "','','" + sFeatures + "')</script>");
        }
        #endregion  打开窗口

        #region 关闭窗口
        //public static void CloseModalDialogWindow(Page thisPage, string sRetureValue)
        //{
        //    thisPage.ClientScript.RegisterStartupScript(typeof(System.String), "", "<stript language='javascript'>window.returnValue='" + sRetureValue + "';self.close();</script>");
        //}
        #endregion 关闭窗口



        #region 显示信息
        /// <summary> 
        /// 服务器端弹出alert对话框 
        /// </summary> 
        /// <param name="str_Message">提示信息,例子："不能为空!"</param> 
        /// <param name="page">Page类</param> 
        public static void Alert(Page page, string sMessage)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript' type='text/javascript' defer='defer'>alert('" + sMessage + "');</script>");
        }

        /// <summary> 
        /// 服务器端弹出alert对话框 
        /// </summary> 
        /// <param name="str_Message">提示信息,例子："不能为空!"</param> 
        /// <param name="page">Page类</param> 
        public  static void Alert(string sMessage)
        {
            HttpContext _context = HttpContext.Current;
            _context.Response.Write("<script language='javascript' type='text/javascript' defer='defer'>");
            _context.Response.Write("window.alert(" + "'" + sMessage.Replace("'", "").Replace("\r", "").Replace("\n", "") + "'" + ")");
            _context.Response.Write("</script>");
        }

        public static void Alert(string sMessage, string Url)
        {
            HttpContext _context = HttpContext.Current;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript' type='text/javascript' defer='defer'>");
            sb.Append("window.alert(" + "'" + sMessage.Replace("'", "").Replace("\r", "").Replace("\n", "") + "'" + ");");
            sb.Append("window.location.href='" + Url + "';");
            sb.Append("</script>");
            _context.Response.Write(sb.ToString());
        }

        public static void ConfirmClose(string sMessage)
        {
            HttpContext _context = HttpContext.Current;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript' type='text/javascript'>");
            sb.Append(@"if(confirm('"+sMessage+"'))  ");
            sb.Append("self.opener=null;self.open('','_self'); self.close();");
            sb.Append("</script>");
            _context.Response.Write(sb.ToString());

        }
        /// <summary> 
        /// 服务器端弹出alert对话框，并使控件获得焦点 
        /// </summary> 
        /// <param name="str_Ctl_Name">获得焦点控件Id值,比如：txt_Name</param> 
        /// <param name="str_Message">提示信息,例子："请输入您姓名!"</param> 
        /// <param name="page">Page类</param> 
        public static void Alert(Page page, string sCtlName, string sMessage)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript' type='text/javascript' defer='defer'>alert('" + sMessage + "');document.forms(0)." + sCtlName + ".focus(); document.forms(0)." + sCtlName + ".select();</script>");
        }
        /// <summary> 
        /// 服务器端弹出confirm对话框 
        /// </summary> 
        /// <param name="str_Message">提示信息,例子："您是否确认删除!"</param> 
        /// <param name="btn">隐藏Botton按钮Id值,比如：btn_Flow</param> 
        /// <param name="page">Page类</param> 
        public static void ConfirmResponse(Page page, string sMessage, string sUrl)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript' type='text/javascript' defer='defer'> if (confirm('" + sMessage + "')==true){ window.location.href = '" + sUrl + "';}</script>");
        }
        /// <summary> 
        /// 服务器端弹出confirm对话框 
        /// </summary> 
        /// <param name="str_Message">提示信息,例子："您是否确认删除!"</param> 
        /// <param name="btn">隐藏Botton按钮Id值,比如：btn_Flow</param> 
        /// <param name="page">Page类</param> 
        public static void Confirm(Page page, string sMessage, string btn)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript' type='text/javascript' defer='defer'> if (confirm('" + sMessage + "')==true){document.forms(0)." + btn + ".click();}</script>");
        }
        /// <summary> 
        ///  服务器端弹出confirm对话框,询问用户准备转向那些操作，包括“确定”和“取消”时的操作 
        /// </summary> 
        /// <param name="str_Message">提示信息，比如："成功增加数据,单击\"确定\"按钮填写流程,单击\"取消\"修改数据"</param> 
        /// <param name="btn_Redirect_Flow">"确定"按钮id值</param> 
        /// <param name="btn_Redirect_Self">"取消"按钮id值</param> 
        /// <param name="page">Page类</param> 
        public static void Confirm(Page page, string sMessage, string btn_Redirect_Flow, string btn_Redirect_Self)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript' type='text/javascript' defer='defer'> if (confirm('" + sMessage + "')==true){document.forms(0)." + btn_Redirect_Flow + ".click();}else{document.forms(0)." + btn_Redirect_Self + ".click();}</script>");
        }

        /// <summary> 
        /// 使控件获得焦点 
        /// </summary> 
        /// <param name="str_Ctl_Name">获得焦点控件Id值,比如：txt_Name</param> 
        /// <param name="page">Page类</param> 
        public static void GetFocus(Page page, string sCtlName)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script language='javascript' type='text/javascript' defer='defer'>document.forms(0)." + sCtlName + ".focus(); document.forms(0)." + sCtlName + ".select();</script>");
        }
        public static void ShowException(System.Exception e)
        {
            if (e.Message.IndexOf("COLUMN REFERENCE 约束") > 0) // 外键冲突 
                ShowMessage(5, "");
            else if (e.Message.IndexOf("PRIMARY KEY 约束") > 0)  //主键冲突
                ShowMessage(6, "");

            else if (e.Message.IndexOf("网络错误") > 0)  //连接中断
                ShowMessage(3, "");
            else
                ShowMessage(e.Message);

        }
        //显示具体信息 
        public static void ShowMessage(string strMessage)
        {
            HttpContext _context = HttpContext.Current;
            _context.Response.Write("<script language='javascript' type='text/javascript'> ");
            _context.Response.Write("window.alert(" + "'" + strMessage.Replace("'", "").Replace("\r", "").Replace("\n", "") + "'" + ")");
            _context.Response.Write("</script>");
        }
        //根据错误号显示信息 
        public static void ShowMessage(int iErrID, string Col_Name)
        {
            string strMessage = Errinfo(iErrID, Col_Name);
            HttpContext _context = HttpContext.Current;
            _context.Response.Write("<script language='javascript' type='text/javascript'> ");
            _context.Response.Write("window.alert(" + "'" + strMessage + "'" + ")");
            _context.Response.Write("</script>");
        }
        private static string Errinfo(int iErrID, string Col_Name)
        {
            switch (iErrID)
            {
                case 0:
                    return "编码冲突: " + Col_Name + " 已经存在";
                case 1:
                    return "外键冲突: " + Col_Name + " 在子表中存在";
                case 2:
                    return "数据库连接失败";
                case 3:
                    return "数据库查询操作失败";

                case 4:
                    return "请输入数字";
                case 5:
                    return "外键冲突," + Col_Name + "操作失败";
                case 6:
                    return "重复主键," + Col_Name + "操作失败";
                case 994:
                    return Col_Name + "操作失败";
                case 995:
                    return Col_Name;
                case 996:
                    return "密码错误";
                case 997:
                    return Col_Name + "不存在";
                case 998:
                    return Col_Name + "不能为空";
                case 999:
                    return "对不起,您没有权限使用";
                case 1000:
                    return Col_Name + "操作成功";
                default:

                    return "";
            }
        }
        #endregion 显示信息
         
        #region 重定向
        /// <summary>
        /// 重定位到新页面
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="page">page</param>
        public static void redirect(Page page, string url)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script>window.location.href='" + url + "';</script>");
        }
        /// <summary>
        /// 服务器端弹出alert对话框,再重定位到新页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sMessage"></param>
        /// <param name="url"></param>
        public static void redirect(Page page, string sMessage, string url)
        {
            page.ClientScript.RegisterStartupScript(typeof(System.String), "", "<script>alert('" + sMessage + "');window.location.href='" + url + "';</script>");
        }
        #endregion 重定向  

        #region 输出js脚本
        public static void WriteJS(string strJS)
        {
           HttpContext _context = HttpContext.Current;
            _context.Response.Write("<script language='javascript' type='text/javascript'> ");
            _context.Response.Write( strJS );
            _context.Response.Write("</script>");
        }
        #endregion  输出js脚本
    }
}
