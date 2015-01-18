using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using com.Model.Base;
using com.Utility;

namespace com.DAL.Base
{
    /// <summary>
    /// XML操作类
    /// 2008-05-02 Created By Handey Pan
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseXmlDAL<T>where T:new()
    {
        private BaseXmlModel<T> _XmlModel = new BaseXmlModel<T>();
        public BaseXmlModel<T> XmlModel
        {
            get { return _XmlModel; }
            set { _XmlModel = value; }
        }
        private int _TimeOut;
        public int TimeOut
        { 
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        private string _FilePath;
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }
        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private string _FullPath = "";
        private string FullPath
        {
            get
            {
                if(string.IsNullOrEmpty( _FullPath))
                    _FullPath = Path.Combine(FilePath, FileName);
                return _FullPath;
            }
        }

        public BaseXmlModel<T> GetXmlModel()
        {
            if (File.Exists(FullPath))
            {
                _XmlModel = SerializeHelper<BaseXmlModel<T>>.DesFromFile(FullPath);
                if (_XmlModel.CreateTime.AddMinutes(TimeOut) < DateTime.Now)
                {
                    CFile.Delete(FullPath);
                    return null;
                }
            }
            else
            {
                return null;
            }
            return _XmlModel;
        }

        public void SaveModel()
        {
            CDirectory.Create(FilePath);
            SerializeHelper<BaseXmlModel<T>>.SerToXml(_XmlModel, FullPath);
        }
    }
}
