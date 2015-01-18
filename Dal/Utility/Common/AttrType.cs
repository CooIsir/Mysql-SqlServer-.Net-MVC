//-------------------------------------------------------------------
//版权所有：版权所有(C) 2006，Microsoft(China) Co.,LTD
//系统名称：GMCC-ADC
//文件名称：AttrType
//模块名称：
//模块编号：
//作　　者：HUANGRUI
//完成日期：12/04/2006 13:45:50
//功能说明：
//-----------------------------------------------------------------
//修改记录：
//修改人：  
//修改时间：
//修改内容：
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using com.jk39.Utility;

namespace com.jk39.Utility
{
    public class AttrType
    {
        public AttrType()
        { 
        }

        public static Dictionary<string, string> GetAttrTypeClass()
        {
            return AttrTypeClass;
        }
        /// <summary>
        /// 扩展属性分类类：Product-产品类，Service-业务类
        /// </summary>
        static public Dictionary<string, string> AttrTypeClass
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("Product", "产品类");
                dic.Add("Service", "业务类");
                dic.Add("BossPrd","Boss产品类");
                return dic;
            }
        }
    }
}
