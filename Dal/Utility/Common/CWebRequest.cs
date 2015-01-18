using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;


namespace com.Utility
{
    public class CWebRequest
    {
        #region 常规操作
        public static string GetPost(RequestArgs mReq)
        {
            string sHtml = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(mReq.Url);
            req.Method = mReq.Method;
            req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
            if (string.IsNullOrEmpty(mReq.charset))
            {
                req.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                req.ContentType = "application/x-www-form-urlencoded; charset="+mReq.charset;
            }
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; Maxthon; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            if (mReq.cookie != string.Empty)
                req.Headers.Add(HttpRequestHeader.Cookie, mReq.cookie);
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            req.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
            req.KeepAlive = true;

            if (mReq.RefererUrl != "")
            {
                req.Referer = mReq.RefererUrl;
            }
            req.AllowAutoRedirect = mReq.blRedirect;
            if (mReq.blPolicy == true)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            if (mReq.Method == "POST")
            {
                StreamWriter sw = null;
                sw = new StreamWriter(req.GetRequestStream());
                sw.Write(mReq.postData);
                sw.Close();
            }
            Encoding encoding = Encoding.UTF8;
            switch (mReq.Encode.ToUpper ())
            {
                case "UTF-8":
                    encoding = Encoding.UTF8;
                    break;
                case "GB2312":
                    encoding = Encoding.GetEncoding("gb2312");
                    break;
                case "BIG5":
                    encoding = Encoding.GetEncoding("BIG5");
                    break;
                default:
                    encoding = Encoding.GetEncoding("gb2312");
                    break;
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream s = resp.GetResponseStream();
            if (resp.ContentEncoding == "gzip")
            {
                byte[] zipBuffer = new byte[1024 * 100];
                int bytesRead = 0;
                int offset = 0;
                while (true)
                {
                    bytesRead = s.Read(zipBuffer, offset, zipBuffer.Length - offset);
                    if (bytesRead <= 0)
                        break;
                    offset += bytesRead;
                }
                MemoryStream ms = new MemoryStream(zipBuffer, 0, offset, true);
                GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress, true);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] deBuffer = new byte[1024 * 300];
                offset = 0;
                while (true)
                {
                    bytesRead = zipStream.Read(deBuffer, offset, 1024);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    offset += bytesRead;
                }
                zipStream.Close();
                sHtml = encoding.GetString(deBuffer, 0, offset);
            }
            else
            {
                StreamReader sr = new StreamReader(s, encoding);
                sHtml = sr.ReadToEnd();
                sr.Close();
            }
            string sHeader = "";
            if (mReq.blGetHeaders == true)
            {
                foreach (string header in resp.Headers)
                    sHeader += header + ":" + resp.Headers[header] + "\r\n";
            }
            sHtml = sHeader + sHtml;
            req.Abort();
            resp.Close();
            return sHtml;
        }
       
        public static byte[] GetBypes(RequestArgs mReq)
        {
            byte[] bHtml = null;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(mReq.Url);
            req.Method = mReq.Method;
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; Maxthon; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            if (mReq.cookie != string.Empty)
                req.Headers.Add(HttpRequestHeader.Cookie, mReq.cookie);
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            req.KeepAlive = true;
            if (mReq.RefererUrl != "")
            {
                req.Referer = mReq.RefererUrl;
            }
            req.AllowAutoRedirect = mReq.blRedirect;
            if (mReq.blPolicy == true)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            if (mReq.Method == "POST")
            {
                StreamWriter sw = null;
                sw = new StreamWriter(req.GetRequestStream());
                sw.Write(mReq.postData);
                sw.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream s = resp.GetResponseStream();

            byte[] zipBuffer = new byte[1024 * 500];
            int bytesRead = 0;
            int offset = 0;
            while (true)
            {
                bytesRead = s.Read(zipBuffer, offset, zipBuffer.Length - offset);
                if (bytesRead <= 0)
                    break;
                offset += bytesRead;
            }
            if (resp.ContentEncoding == "gzip")
            {
                MemoryStream ms = new MemoryStream(zipBuffer, 0, offset, true);
                GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress, true);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] deBuffer = new byte[1024 * 300];
                offset = 0;
                while (true)
                {
                    bytesRead = zipStream.Read(deBuffer, offset, 1024);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    offset += bytesRead;
                }
                zipStream.Close();
                bHtml = new byte[offset];
                deBuffer.CopyTo(bHtml, offset);
            }
            else
            {
                bHtml = new byte[offset];
                for (int i = 0; i < offset; i++)
                    bHtml[i] = zipBuffer[i];
            }
            req.Abort();
            resp.Close();
            return bHtml;
        }
     
        public static string GetHead(string strUrl)
        {
            string sHtml = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, */*";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; Maxthon; .NET CLR 1.1.4322;)";
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            req.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
            req.KeepAlive = true;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            foreach (string header in resp.Headers)
            {
                sHtml += header + ":" + resp.Headers[header] + "\r\n";
            }
            req.Abort();
            resp.Close();
            return sHtml;
        }
        #endregion  常规操作
         
        #region 辅助函数 
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        } 
        #endregion 辅助函数
    }
}