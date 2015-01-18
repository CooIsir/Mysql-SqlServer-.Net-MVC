using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.manager;

namespace AjaxList
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = Dal.Manager.Manager.MManageDal.MysqlGetDataReader("", "");
            GridView1.DataBind();
            com.manager.manager test=new manager();
           Response.Write(test.UserName);
        }
    }
}