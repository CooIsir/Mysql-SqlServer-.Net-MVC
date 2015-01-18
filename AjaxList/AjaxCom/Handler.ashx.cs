using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;

namespace AjaxList.AjaxCom
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    ///  
    class MysqQuery:MysqlUsingDll.UsingMysql
    {
  
        public static bool GetDataSet(string sql)
        {
            string myconnStr = ConfigurationManager.ConnectionStrings["CoolsirMySqlstr"].ToString();
            using (MySqlDataAdapter sda = QueryMysqlDataAdapter(sql,myconnStr))
            {
               
                    var ds = new DataSet();
                    sda.Fill(ds, "Test");

                    if (ds.Tables["Test"].Rows.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
              
            }
        }
        public static DataTable GetDataList(string sql)
        {
            string myconnStr = ConfigurationManager.ConnectionStrings["CoolsirMySqlstr"].ToString();
            using (MySqlDataAdapter sda = QueryMysqlDataAdapter(sql, myconnStr))
            {

                var ds = new DataSet();
                sda.Fill(ds, "Test");

                if (ds.Tables["Test"].Rows.Count != 0)
                {
                    return ds.Tables["Test"];
                }
                else
                {
                    return null;
                }

            }
        }
    }
    public class Handler : IHttpHandler
    {

        private HttpContext _context;
        public void ProcessRequest(HttpContext context)
        {

            this._context = context;
            string cmd = context.Request.QueryString["cmd"] == null ? "" : context.Request.QueryString["cmd"].ToString();
            //获取操作符类型;
            StringBuilder resString=new StringBuilder();
            switch (cmd)
            {
                case "listShow":                        //请求显示所有信息
                    DataTable dt=new DataTable();
                    String sql = "select * from commodities";
                    dt = MysqQuery.GetDataList(sql);
                    String res = MakeAlllistShow(dt);
                    resString.Append(res);
                    break;
                case "ShopInfo":                        //请求显示详细信息
                    string id = _context.Request.Form["id"].ToString();
                    DataTable dt2=new DataTable();
                    String sql2 = String.Format("select * from commodities where id={0}",id);
                    dt2 = MysqQuery.GetDataList(sql2);
                    String res2 = MakeAlllistShow(dt2);
                    resString.Append(res2);
                    break;
                default:
                    break;

            }
          //  Model.Manager. manager = Dal.Manager.Manager.MManageDal.GetModel(1);
            com.manager.manager manager = Dal.Manager.Manager.MManageDal.GetModel(1);
            context.Response.ContentType = "text/plain";
            context.Response.Write(manager.UserName);                   //回应信息;
        }

        public String MakeAlllistShow(DataTable dt)             //从dt提取数据;
        {
            StringBuilder resSB=new StringBuilder();
            if (dt != null)
            {
                resSB.Append("<ul>");
                foreach (DataRow mDr in dt.Rows)
                {
                    string listID = mDr[0].ToString();
                    string imgsrc = mDr["ComImages"].ToString();

                    string newli = MakeLianda(imgsrc, listID);
                    resSB.Append(newli);
                }
                resSB.Append("</ul>");
            }
            return resSB.ToString();                                //返回构建ok的ul标签;
        }

        public String MakeLianda(String res,String id)              //构建Li标签
        {
            return "<li>" + "<a href=\"./ShopInfo.html"+ "?id=" + id + "\">" + "<img src=\"" + res +
                   "\"/></li>";
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}