using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.Utility
{
    public  class CDirectory
    {
        public static void Delete(string sPath)
        {
            string[] strFiles = Directory.GetFiles(sPath);
            foreach (string strFile in strFiles)
            {
                try
                {
                    CFile.Delete(strFile);
                }
                catch { }
            }
            string[] strDirs = Directory.GetDirectories(sPath);            
            foreach (string strDir in strDirs)
            {
                try
                {
                    Delete(strDir);
                }
                catch { }
            }
            FileInfo f = new FileInfo(sPath);
            f.Attributes = FileAttributes.Normal;
            Directory.Delete(sPath);
        }
        public static void Create(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    return;
                if (Directory.Exists(path))
                    return;
                Directory.CreateDirectory(path);
            }
            catch { }
        }
        public static string GetPathByWeb(string sWebPath)
        { 
            if (sWebPath == "")
                throw new Exception("<font color='red'>未指定路径</font>");
            int iIndex = sWebPath.LastIndexOf("/");
            if (iIndex < 0)
                throw new Exception("font color='red'>指定路径格式不正确</font>");
            string sFileName = sWebPath.Substring(iIndex + 1);
            if (sFileName.IndexOf('.') <= 0)
                throw new Exception("<font color='red'>文件名格式不对</font>");
            string sPath = sWebPath.Substring(0, sWebPath.LastIndexOf("/") + 1);
            sPath = System.Web.HttpContext.Current.Server.MapPath(sPath);

            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            if(!sPath.EndsWith("\\"))
            {
                sPath+="\\";
            }
            return sPath + sFileName;            
        }

        public static List<string> GetDirectories(string sPath)
        {
            return GetDirectories(sPath, true);
        }

        public static List<string> GetDirectories(string sPath, bool IsFull)
        {   
            string[] strDir = null;
            try
            {
                strDir = Directory.GetDirectories(sPath);
            }
            catch { }
            List<string> listDir = new List<string>();
            if (strDir != null)
            {
                foreach (string s in strDir)
                {
                    if (IsFull)
                        listDir.Add(s);
                    else
                        listDir.Add(s.Replace(sPath, "").TrimStart('\\', '/'));
                }
            }
            return listDir;
        }

        public static List<string> GetFiles(string sPath)
        {
            return GetFiles(sPath, true);
        }

        public static List<string> GetFiles(string sPath, bool IsFull)
        {
            string[] strFile = null;
            try
            {
                strFile = Directory.GetFiles(sPath);
            }
            catch { }
            List<string> listFile = new List<string>();
            if (strFile != null)
            {
                foreach (string s in strFile)
                {
                    if (IsFull)
                        listFile.Add(s);
                    else
                        listFile.Add(s.Replace(sPath, "").TrimStart('\\','/'));
                }
            }
            return listFile;
        }
    }
}
