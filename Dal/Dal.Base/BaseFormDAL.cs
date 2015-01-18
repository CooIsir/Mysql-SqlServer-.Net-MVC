using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.DAL.Base
{
    /// <summary>
    /// 表单数据源操作基类
    /// </summary>
    public class BaseFormDAL<T>
        where T : new()
    {
        /// <summary>
        /// 读取表单字符串值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ParamString(string name)
        {
            string value = null;
            value = System.Web.HttpContext.Current.Request.Form[name];
            if (value == null)
            {
                value = System.Web.HttpContext.Current.Request.Params[name];
            }
            if (value == null)
            {
                value = "";
            }
            return value;
        }

        /// <summary>
        /// 读取表单整型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int ParamInt(string name, int defaultValue)
        {
            int value = defaultValue;
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[name]))
            {
                int.TryParse( System.Web.HttpContext.Current.Request.Params[name],out value);
            }
            else
            {
                int.TryParse(System.Web.HttpContext.Current.Request.Form[name],out value);
            }
            return value;
        }


        /// <summary>
        /// 读取表单布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool ParamBool(string name)
        {
            bool value = false;
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[name]))
            {
                bool.TryParse(System.Web.HttpContext.Current.Request.Params[name], out value);
            }
            else
            {
                bool.TryParse(System.Web.HttpContext.Current.Request.Form[name], out value);
            }
            return value;
        }


        /// <summary>
        /// 读取表单double型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double ParamDouble(string name)
        {
            double value = 0;
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[name]))
            {
                double.TryParse(System.Web.HttpContext.Current.Request.Params[name], out value);
            }
            else
            {
                double.TryParse(System.Web.HttpContext.Current.Request.Form[name], out value);
            }
            return value;
        }


        /// <summary>
        /// 读取表单上传文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public System.Web.HttpPostedFile ParamFile(string name)
        {
            System.Web.HttpPostedFile file = null;
            if (System.Web.HttpContext.Current.Request.Files[name] != null)
            {
                if (System.Web.HttpContext.Current.Request.Files[name].FileName != "")
                    file = System.Web.HttpContext.Current.Request.Files[name];
            }
            return file;
        }


        /// <summary>
        /// 得到实体
        /// </summary>
        /// <returns></returns>
        public T GetModel()
        {
            T model = new T();
            System.Reflection.FieldInfo[] fis = typeof(T).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo fi in fis)
            {
                try
                {
                    string value = ParamString(fi.Name);
                    if (value != null)
                    {
                        if (fi.FieldType == typeof(int))
                        {
                            fi.SetValue(model, int.Parse(value));
                        }
                        else if (fi.FieldType == typeof(string))
                        {
                            fi.SetValue(model, value);
                        }
                        else if (fi.FieldType == typeof(bool))
                        {
                            fi.SetValue(model, ParamBool(fi.Name));
                        }
                        else if (fi.FieldType == typeof(double))
                        {
                            fi.SetValue(model, ParamDouble(fi.Name));
                        }
                        else if (fi.FieldType == typeof(System.Web.HttpPostedFile))
                        {
                            fi.SetValue(model, ParamFile(fi.Name));
                        }

                    }
                }
                catch { }
            }
            return model;
        }
    }
}
