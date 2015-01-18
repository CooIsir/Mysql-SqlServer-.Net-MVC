using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace com.Utility
{
    public class ImgHelp
    {
        public ImgHelp() { }

        #region 属性
        /// <summary>
        /// 可以上传的文件类型
        /// </summary>
        public static string VaildUploadType
        {
            get
            {
                return "/.jpg/.gif/.zip/";
            }
        }
        /// <summary>
        /// 有效的图片格式
        /// </summary>
        public static string VaildImageType
        {
            get
            {
                return "/.jpg/.gif/";
            }
        }
        #endregion 属性

        /// <summary>
        /// 根据图片扩展名返回图片格式
        /// </summary>
        /// <param name="sExtension"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat GetImageType(string sExtension)
        {
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.MemoryBmp;
            switch (sExtension)
            {
                case ".jpg":
                    format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case ".gif":
                    format = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
            }

            return format;
        }

        #region 获得图片保存路径
        /// <summary>
        /// 获取图片保存的目录,如日期为2007-1-1,则目录为/2007/1/1/
        /// </summary>
        public static string GetImageDirectory()
        {
            string sPath = "";
            DateTime dtNow = DateTime.Now;
            sPath = string.Format("/{0}/{1}/{2}/", dtNow.Year, dtNow.Month, dtNow.Day);
            return sPath;
        }
        public static string CreateFileName()
        {
            string sName = "";
            DateTime dtNow = DateTime.Now;
            sName = string.Format("{0:x}", dtNow.Ticks).Substring(4);
            return sName;
        }
        /// <summary>
        /// 文件名使用Tick的十六进制表示(去除了前四位)作为文件名
        /// </summary>
        public static string CreateFileName(string sExtention)
        {
            string sName = "";
            DateTime dtNow = DateTime.Now;
            sName = string.Format("{0:x}", dtNow.Ticks).Substring(4);
            return sName + sExtention;
        }
        public static string CreateFileName(string sExtention,int iFlag)
        {
            string sName = "";
            DateTime dtNow = DateTime.Now;
            sName = string.Format("{0:x}", dtNow.Ticks).Substring(4);
            return sName + iFlag + sExtention;
        }

        #endregion 获得图片保存路径

        #region 保存图片
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="inputStream">输入数据流</param>
        /// <param name="sRootDirectory">保存的根目录(不要包括根据时间自动生成的目录)</param>
        /// <param name="sExtension">扩展名</param>
        /// <returns>返回根据时间自动生成的文件路径</returns>
        public static string SaveImage(Stream inputStream, string sRootDirectory, string sExtension)
        {
            sRootDirectory = sRootDirectory.TrimEnd('/', '\\');

            string sReturn = GetImageDirectory() + CreateFileName(sExtension);
            string path = sRootDirectory + sReturn;
            CDirectory.Create(Path.GetDirectoryName(path));
            using (FileStream fStream = File.Create(path))
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = inputStream.Read(data, 0, data.Length);
                    if (size > 0)
                        fStream.Write(data, 0, size);
                    else
                        break;
                }
            }
            return sReturn;
        }

        public static void SaveImage(Stream inputStream, string sFilePath)
        {
            CDirectory.Create(Path.GetDirectoryName(sFilePath));
            using (FileStream fStream = File.Create(sFilePath))
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = inputStream.Read(data, 0, data.Length);
                    if (size > 0)
                        fStream.Write(data, 0, size);
                    else
                        break;
                }
            }
        }

        /// <summary>
        /// 保存缩略图片
        /// </summary>
        /// <param name="inputStream">输入数据流</param>
        /// <param name="sRootDirectory">保存的根目录(不要包括根据时间自动生成的目录)</param>
        /// <param name="sExtension">扩展名</param>
        /// <param name="iWidth">图片宽度（缩略后）</param>
        /// <param name="iHeight">图片高度（缩略后）</param>
        /// <returns></returns>
        public static string SaveThumbnailImage(Stream inputStream, string sRootDirectory, string sExtension, int iWidth, int iHeight)
        {
            Image image = Image.FromStream(inputStream);
            image = image.GetThumbnailImage(iWidth, iHeight, null, System.IntPtr.Zero);

            MemoryStream newStream = new MemoryStream();
            image.Save(newStream, GetImageType(sExtension));
            newStream.Seek(0, SeekOrigin.Begin);
            
            //保存
            sRootDirectory = sRootDirectory.TrimEnd('/', '\\');

            string sReturn = GetImageDirectory() + "T" + CreateFileName(sExtension);
            string path = sRootDirectory + sReturn;
            CDirectory.Create(Path.GetDirectoryName(path));
            using (FileStream fStream = File.Create(path))
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = newStream.Read(data, 0, data.Length);
                    if (size > 0)
                        fStream.Write(data, 0, size);
                    else
                        break;
                }
            }
            return sReturn;
        }
        #endregion 保存图片

        #region 解压数据包并保存图片
        /// <summary>
        /// 解压zip数据包并保存图片
        /// </summary>
        /// <param name="inputStream">输入数据流</param>
        /// <param name="sDirectory">保存的根目录(不要包括根据时间自动生成的目录)</param>
        /// <returns>返回根据时间自动生成的文件路径列表</returns>
        //public static List<string> UnzipImage(Stream inputStream,string sRootDirectory)
        //{
            //sRootDirectory = sRootDirectory.TrimEnd('/', '\\');

            //List<string> list = new List<string>();
            //using (ZipInputStream s = new ZipInputStream(inputStream))
            //{
            //    //为了防止图片文件名重复,在文件名后添加一个标识
            //    int i=0;
            //    ZipEntry theEntry;
            //    while ((theEntry = s.GetNextEntry()) != null)
            //    {
            //        string directoryName = Path.GetDirectoryName(theEntry.Name);
            //        string fileName = Path.GetFileName(theEntry.Name);
            //        string extension = Path.GetExtension(theEntry.Name);

            //        if (fileName != string.Empty)
            //        {
            //            if (VaildImageType.IndexOf("/" + extension + "/") == -1)
            //            {
            //                continue;
            //            }
            //            string sReturn = GetImageDirectory() + CreateFileName(extension, i++);
            //            string path = sRootDirectory + @"\" + sReturn;
            //            CDirectory.Create(Path.GetDirectoryName(path));
            //            using (FileStream fStream = File.Create(path))
            //            {
            //                int size = 2048;
            //                byte[] data = new byte[2048];
            //                while (true)
            //                {
            //                    size = s.Read(data, 0, data.Length);
            //                    if (size > 0)
            //                        fStream.Write(data, 0, size);
            //                    else
            //                        break;
            //                }
            //            }
            //            list.Add(sReturn);
            //        }
            //    }
            //}
            //return list;
        //}
        #endregion 解压数据包并保存图片
    }
}