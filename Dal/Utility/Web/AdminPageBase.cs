using System;
using System.IO;
using System.Web.UI;

namespace com.Utility
{
    /// <summary> 
    /// PageBase 的摘要说明。  页面基础类 个资源的维护页面都继承至该页面  主要以Basic/District.aspx.cs为例 做详细说明
    /// 其他资源维护页面函数名称  接口  流程  完全一致   只是具体的语句根据各页面的控件的不同而不同
    /// </summary>
    public class AdminPageBase : Page
    {
        //各页面装载时所必须装载的函数  
        //public virtual void PageLoad()
        //{
        //    CheckUserToken();
        //    if (!IsPostBack) //如果是第一次加载页面 
        //    {
        //        BindData(); //绑定数据
        //    }
        //    else
        //    {
        //        btnAction();
        //    }
        //    BindData();
        //}

        /// <summary>
        /// 检查用户标识
        /// </summary>
        //protected virtual void CheckUserToken() { }

        ////  绑定页面的数据，由于各页面相差很大，故必须重写
        //protected virtual void BindData(){ }
        //protected virtual void btnAction()
        //{
        //    switch (Request["mAction"])
        //    {
        //        case "ActionSel":
        //            btnSel();
        //            break;
        //        case "ActionAdd":
        //            btnAdd();
        //            break;
        //        case "ActionEdit":
        //            btnEdit();
        //            break;
        //        case "ActionDel":
        //            btnDel();
        //            break;
        //        case "ActionAudit":
        //            btnAudit();
        //            break;
        //        case "ActionSave":
        //            btnSave();
        //            break;
        //        case "ActionReturn":
        //            btnReturn();
        //            break;
        //        case "ActionAddChild":
        //            btnAddChild();
        //            break;
        //        case "ActionRestore":
        //            btnRestore();
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //protected virtual void btnSel(){}
        //protected virtual void btnAdd(){}
        //protected virtual void btnEdit(){}
        //protected virtual void btnDel(){}
        //protected virtual void btnAudit(){}
        //protected virtual void btnSave(){}
        //protected virtual void btnReturn(){}
        //protected virtual void btnAddChild(){}
        //protected virtual void btnRestore(){}
       
        public virtual string mAction
        {
            get { return ViewState["mAction"].ToString(); }
        }
        public virtual string SortField
        {
            get
            {
                object oSortField = ViewState["gvSortField"];
                if (oSortField == null)
                    return "ID";
                else
                    return oSortField.ToString();
            }
            set { ViewState["gvSortField"] = value; }
        }
        public virtual bool SortType
        {
            get
            {
                object oSortField = ViewState["gvSortType"];
                if (oSortField == null)
                    return true;
                else
                    return (bool) oSortField;
            }
            set { ViewState["gvSortType"] = value; }
        }       
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            Pair pair;
            PageStatePersister persister = this.PageStatePersister;
            object viewState;
            if (state is Pair)
            {
                pair = (Pair) state;
                persister.ControlState = pair.First;
                viewState = pair.Second;
            }
            else
            {
                viewState = state;
            }

            LosFormatter formatter = new LosFormatter();
            StringWriter writer = new StringWriter();
            formatter.Serialize(writer, viewState);
            string viewStateStr = writer.ToString();
            byte[] data = Convert.FromBase64String(viewStateStr);
            byte[] compressedData = zipHelper.Compress(data);
            string str = Convert.ToBase64String(compressedData);

            persister.ViewState = str;
            persister.Save();
        }
        protected override object LoadPageStateFromPersistenceMedium()
        {
            PageStatePersister persister = this.PageStatePersister;
            persister.Load();

            string viewState = persister.ViewState.ToString();
            byte[] data = Convert.FromBase64String(viewState);
            byte[] uncompressedData = zipHelper.DeCompress(data);
            string str = Convert.ToBase64String(uncompressedData);
            LosFormatter formatter = new LosFormatter();
            return new Pair(persister.ControlState, formatter.Deserialize(str));
        }
    }
}