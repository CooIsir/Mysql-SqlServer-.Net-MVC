using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net ;
using System.Net.Sockets ;

namespace com.Utility
{
    public class CSocket
    {
        public static string GetPost(RequestArgs mArgs)
        {
            //(1)创建IPEndPoint实例和套接字
            IPAddress hostIp = Dns.GetHostEntry(mArgs.IpAddress).AddressList[0];
            IPEndPoint ep = new IPEndPoint(hostIp, 80);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //(2)连接服务器端
            try
            {
                client.Connect(ep);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }

            StringBuilder sbRequest = new StringBuilder();
            Uri u = new Uri(mArgs.Url);
            sbRequest.AppendLine(string.Format("{0} {1} HTTP/1.1", mArgs.Method, u.PathAndQuery));



            if (string.IsNullOrEmpty(mArgs.Accept))
            {
                sbRequest.AppendLine("Accept: image/gif, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
            }
            else
            {
                sbRequest.AppendLine(string.Format("Accept: {0}", mArgs.Accept));
            }


            sbRequest.AppendLine("Accept-Language: zh-cn");
            sbRequest.AppendLine("User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30618; InfoPath.2)");
            sbRequest.AppendLine("Accept-Encoding: gzip, deflate");
            sbRequest.AppendLine(string.Format("Host: {0}", u.Host));
            if (!string.IsNullOrEmpty(mArgs.RefererUrl))
            {
                sbRequest.AppendLine(string.Format("Referer: {0}", mArgs.RefererUrl));
            }

            if (!string.IsNullOrEmpty(mArgs.ContentType))
            {
                sbRequest.AppendLine(string.Format("Content-Type: {0}", mArgs.ContentType));
            }

            sbRequest.AppendLine("Connection: Keep-Alive");
            if (mArgs.Method == "POST")
            {
                sbRequest.AppendLine(string.Format("Content-Length: {0}", mArgs.postData.Length));
            }
            sbRequest.AppendLine("");

            if (mArgs.Method == "POST")
            {
                sbRequest.AppendLine(mArgs.postData);
                sbRequest.AppendLine("");
            }

            //(3)发送请求
            client.Send(Encoding.ASCII.GetBytes(sbRequest.ToString()));

            //(4)接收数据
            StringBuilder recstr = new StringBuilder();
            byte[] buff = new byte[1024 * 3];

            int rCount = 0;

            while (true)
            {
                rCount = client.Receive(buff, buff.Length, SocketFlags.None); //读取数据 
                if (rCount > 0)
                {
                    recstr.Append(Encoding.GetEncoding(mArgs.Encode).GetString(buff, 0, rCount));
                    System.Threading.Thread.Sleep(50);
                }
                if (rCount <= buff.Length)
                {
                    break;
                }
            }
            client.Close();
            if (mArgs.blGetHeaders)
            {
                return recstr.ToString();
            }
            else
            {
                return CRegex.Replace(recstr.ToString(), @"^HTTP[\s\S]+?(\r\n){2,}", "", 0);
            }
        }
    }
}
