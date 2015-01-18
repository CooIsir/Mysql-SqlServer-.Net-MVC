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

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace com.Utility
{
    /// <summary>
    /// SerializationHelper 的摘要说明。
    /// </summary>
    public class SerializeHelper<T> where T : new()
    {
        private SerializeHelper() { }

        #region 序列化成xml
        /// <summary>
        /// 反序列化--从字符串
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="strXml">xml内容</param>
        /// <returns></returns>
        public static T DesFromXml(string strXml)
        {
            T model = default(T);
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
        /// 反序列化--从文件
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filePath">硬盘文件绝对路径</param>
        /// <returns></returns>
        public static T DesFromFile(string filePath)
        {
            T model = new T();
            FileStream fs = null;
            try
            {
                //textReader = new StringReader(filePath);
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                model = (T)serializer.Deserialize(fs);
            }
            catch
            {
                model = new T();
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return model;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        public static string SerToXml(T model)
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
            catch //(Exception Ex)
            {
                //System.Diagnostics.Trace.Write(Ex.Message);
            }
            finally
            {
                if (textwriter != null)
                    textwriter.Close();
            }
            return sReturn;
        }
        /// <summary>
        /// 序列化到文件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void SerToXml(T model, string filePath)
        {
            FileStream fs = null;
            string sReturn = "";
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, model);
                sReturn = fs.ToString();
            }
            catch
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        #endregion 序列化成xml

        #region 二进制序列化
        public static string SerToBin(T model)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] bt=null;
            try
            {
                formatter.Serialize(ms, model);
                ms.Seek(0, SeekOrigin.Begin);
                bt = new byte[ms.Length];
                ms.Read(bt, 0, bt.Length);
            }
            catch { }
            finally
            {
                ms.Close();
            }
            return System.Convert.ToBase64String(bt);
        }
        public static T DesFromBin(string strBase64)
        {
            T obj = default(T);
            IFormatter formatter = new BinaryFormatter();
            byte[] bt = System.Convert.FromBase64String(strBase64);
            MemoryStream ms = new MemoryStream(bt);
            try
            {
                obj = (T)formatter.Deserialize(ms);
            }
            catch
            {
                obj = new T();
            }
            finally
            {
                ms.Close();
            }
            return obj;
        }
        #endregion  二进制序列化
    }
}
