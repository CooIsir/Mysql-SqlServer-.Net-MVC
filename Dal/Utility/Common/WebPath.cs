using System;
using System.Collections.Generic;
using System.Text;

namespace com.Utility
{
    public class WebPath
    {
        #region WebSitePath
        /// <summary>
        /// 六库后台路径
        /// </summary>
        public static string WebManagePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebManagePath"];
            }
        }
        /// <summary>
        /// 发布后台路径
        /// </summary>
        public static string PublishPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PublishPath"];
            }
        }
        /// <summary>
        /// 药厂路径
        /// </summary>
        public static string CompanyPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CompanyPath"];
            }
        }
        /// <summary>
        /// 化妆品
        /// </summary>
        public static string CosmeticPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CosmeticPath"];
            }
        }
        /// <summary>
        /// 疾病
        /// </summary>
        public static string DiseasePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DiseasePath"];
            }
        }
        /// <summary>
        /// 检查
        /// </summary>
        public static string CheckPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CheckPath"];
            }
        }
        /// <summary>
        /// 症状
        /// </summary>
        public static string SymptomPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SymptomPath"];
            }
        }
        /// <summary>
        /// 医生
        /// </summary>
        public static string DoctorPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DoctorPath"];
            }
        }
        /// <summary>
        /// 药品
        /// </summary>
        public static string DrugPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DrugPath"];
            }
        }
        /// <summary>
        /// 医院
        /// </summary>
        public static string HospitalPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HospitalPath"];
            }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public static string DepartmentPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DepartmentPath"];
            }
        }
        /// <summary>
        /// 导医
        /// </summary>
        public static string DyPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DyPath"];
            }
        }
        /// <summary>
        /// 图片保存路径
        /// </summary>
        public static string ImagePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ImagePath"];
            }
        }
        /// <summary>
        /// 自测库
        /// </summary>
        public static string TestPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TestPath"];
            }
        }
        /// <summary>
        /// 北京分站
        /// </summary>
        public static string BeijingPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["BeijingPath"];
            }
        }
        /// <summary>
        /// 广州分站
        /// </summary>
        public static string GZPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GZPath"];
            }
        }
        /// <summary>
        /// 深圳分站
        /// </summary>
        public static string SZPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SZPath"];
            }
        }
        /// <summary>
        /// 上海分站
        /// </summary>
        public static string ShanghaiPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ShanghaiPath"];
            }
        }
        /// <summary>
        /// 发布服务
        /// </summary>
        public static string ServicedbPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ServicedbPath"];
            }
        }
        /// <summary>
        /// 整形
        /// </summary>
        public static string ZhengxingPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ZhengxingPath"];
            }
        }
        /// <summary>
        /// 性百科
        /// </summary>
        public static string XingBaiKePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["XinBaiKePath"];
            }
        }
        /// <summary>
        /// 招商
        /// </summary>
        public static string CommercialPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CommercialPath"];
            }
        }
        /// <summary>
        /// 药店
        /// </summary>
        public static string YdkPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["YdkPath"];
            }
        }
        /// <summary>
        /// 药商
        /// </summary>
        public static string YygsPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["YygsPath"];
            }
        }
        #endregion WebSitePath
    }
}
