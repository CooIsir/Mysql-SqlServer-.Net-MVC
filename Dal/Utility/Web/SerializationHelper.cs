/*
 * Discuz!NT Version: 1.0
 * Created on 2007-3-30
 *
 * Web: http://www.discuznt.com
 * Copyright (C) 2001 - 2007 Comsenz Technology Inc., All Rights Reserved.
 * This is NOT a freeware, use is subject to license terms.
 *
 */


using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace com.jk39.Utility
{
    /// <summary>
    /// SerializationHelper 的摘要说明。
    /// </summary>
    public class SerializationHelper<T> where T:new ()
    {
        private SerializationHelper()
        {

        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="strXml">xml内容</param>
        /// <returns></returns>
        public static T Deserialize(string strXml)
        {
            T model = new T();
            TextReader textReader = null;
            try
            { 
                textReader = new StringReader(strXml);              
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                model = (T)serializer.Deserialize(textReader);                
            }
            catch  
            {
                model = new T();
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }
            return model;
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static string Serialize(T model)
        {
            TextWriter textwriter = null;
            string sReturn = "";
            try
            {
                textwriter = new StringWriter();                
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(textwriter, model);
                sReturn = textwriter.ToString();
            }
            catch  
            {
                 
            }
            finally
            {
                if (textwriter != null)
                    textwriter.Close();
            }
            return sReturn;
        }
    }
}
