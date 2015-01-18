using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace com.Utility
{
    public class CEMailData
    {
        /// <summary>
        /// 设定语言代码，默认设定为GB2312，如不需要可设置为""
        /// </summary>
        public string Charset = "GB2312";

        /// <summary>
        /// 发件人地址，默认值：ggwscdc@163.com
        /// </summary>
        public string From = "ggwscdc@163.com";

        /// <summary>
        /// 发件人姓名 默认值：39健康网
        /// </summary>
        public string FromName = "湖北疾控";

        /// <summary>
        /// 回复邮件地址，默认值: ggwscdc@163.com
        /// </summary>
        public string ReplyTo = "ggwscdc@163.com";

        /// <summary>
        /// 邮件服务器域名
        /// </summary>	
        protected string mailserver;

        /// <summary>
        /// 邮件服务器域名和验证信息
        /// 形如："user:pass@www.server.com:25"，也可省略次要信息。如"user:pass@www.server.com"或"www.server.com"
        /// </summary>	
        public string MailDomain
        {
            set
            {
                string maidomain = value.Trim();

                if (maidomain != "")
                {
                    int tempint = maidomain.LastIndexOf("@");
                    if (tempint != -1)
                    {
                        string up = maidomain.Substring(0, tempint);
                        MailServerUserName = up.Substring(0, up.IndexOf(":"));
                        MailServerPassWord = up.Substring(up.IndexOf(":") + 1, up.Length - up.IndexOf(":") - 1);
                        maidomain = maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1);
                    }

                    tempint = maidomain.IndexOf(":");
                    if (tempint != -1)
                    {
                        mailserver = maidomain.Substring(0, tempint);
                        mailserverport = Convert.ToInt32(maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1));
                    }
                    else
                    {
                        mailserver = maidomain;
                    }
                }
            }
        }

        /// <summary>
        /// 邮件服务器端口号
        /// </summary>	
        protected int mailserverport = 25;

        /// <summary>
        /// 邮件服务器端口号
        /// </summary>	
        public int MailDomainPort
        {
            set { mailserverport = value; }
        }


        /// <summary>
        /// 是否需要SMTP验证
        /// </summary>		
        protected bool ESmtp = false;

        /// <summary>
        /// SMTP认证时使用的用户名
        /// </summary>
        protected string username = "";

        /// <summary>
        /// SMTP认证时使用的用户名
        /// </summary>
        public string MailServerUserName
        {
            set
            {
                if (value.Trim() != "")
                {
                    username = value.Trim();
                    ESmtp = true;
                }
                else
                {
                    username = "";
                    ESmtp = false;
                }
            }
        }

        /// <summary>
        /// SMTP认证时使用的密码
        /// </summary>
        protected string password = "";

        /// <summary>
        /// SMTP认证时使用的密码
        /// </summary>
        public string MailServerPassWord
        {
            set { password = value; }
        }

        /// <summary>
        /// 邮件主题
        /// </summary>		
        public string Subject = "";

        /// <summary>
        /// 是否Html邮件
        /// </summary>		
        public bool Html = false;

        /// <summary>
        /// 收件人是否发送收条
        /// </summary>
        public bool ReturnReceipt = false;

        /// <summary>
        /// 邮件正文
        /// </summary>		
        public string Body = "";


        /// <summary>
        /// 收件人最大数量：现在很多SMTP都限制收件人的最大数量，以防止广告邮件泛滥，最大数量一般都限制在10个以下。
        /// </summary>
        protected int RecipientMaxNum = 10;

        /// <summary>
        /// 收件人列表
        /// </summary>
        protected ArrayList Recipient = new ArrayList();

        /// <summary>
        ///抄送收件人列表
        /// </summary>
        protected ArrayList RecipientCC = new ArrayList();

        /// <summary>
        /// 密送收件人列表
        /// </summary>
        protected ArrayList RecipientBCC = new ArrayList();

        /// <summary>
        /// 邮件发送优先级，可设置为"High","Normal","Low"或"1","3","5"
        /// </summary>
        protected string priority = "Normal";

        /// <summary>
        /// 邮件发送优先级，可设置为"High","Normal","Low"或"1","3","5"
        /// </summary>
        public string Priority
        {
            set
            {
                switch (value.ToLower())
                {
                    case "high":
                        priority = "High";
                        break;

                    case "1":
                        priority = "High";
                        break;

                    case "normal":
                        priority = "Normal";
                        break;

                    case "3":
                        priority = "Normal";
                        break;

                    case "low":
                        priority = "Low";
                        break;

                    case "5":
                        priority = "Low";
                        break;

                    default:
                        priority = "Normal";
                        break;
                }
            }
        }


        /// <summary>
        /// 错误消息反馈
        /// </summary>
        protected string errmsg;

        /// <summary>
        /// 错误消息反馈
        /// </summary>		
        public string ErrorMessage
        {
            get { return errmsg; }
        }


        /// <summary>
        /// 服务器交互记录
        /// </summary>
        protected string logs = "";

        /// <summary>
        /// 服务器交互记录，如发现本组件不能使用的SMTP服务器，请将出错时的Logs发给我（huolx@s2008.com），我将尽快查明原因。
        /// </summary>
        public string Logs
        {
            get { return logs; }
        }

        /// <summary>
        /// 服务器交互记录(html格式)，如发现本组件不能使用的SMTP服务器，请将出错时的Logs发给我（huolx@s2008.com），我将尽快查明原因。
        /// </summary>
        public string HtmlLogs
        {
            get { return logs.Replace("<", "&lt;").Replace(">", "&gt;").Replace(enter, "<br>").Replace(" ", "&nbsp;"); }
        }

        protected string enter = "\r\n";

        /// <summary>
        /// TcpClient对象，用于连接服务器
        /// </summary>	
        protected TcpClient tc;

        /// <summary>
        /// NetworkStream对象
        /// </summary>	
        protected NetworkStream ns;

        /// <summary>
        /// SMTP错误代码哈希表
        /// </summary>
        protected Hashtable ErrCodeHT = new Hashtable();

        /// <summary>
        /// SMTP正确代码哈希表
        /// </summary>
        protected Hashtable RightCodeHT = new Hashtable();

        /// <summary>
        /// SMTP回应代码哈希表
        /// </summary>
        protected void SMTPCodeAdd()
        {
            ErrCodeHT.Clear();
            RightCodeHT.Clear();
            ErrCodeHT.Add("500", "邮箱地址错误");
            ErrCodeHT.Add("501", "参数格式错误");
            ErrCodeHT.Add("502", "命令不可实现");
            ErrCodeHT.Add("503", "服务器需要SMTP验证");
            ErrCodeHT.Add("504", "命令参数不可实现");
            ErrCodeHT.Add("421", "服务未就绪，关闭传输信道");
            ErrCodeHT.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）");
            ErrCodeHT.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）");
            ErrCodeHT.Add("451", "放弃要求的操作；处理过程中出错");
            ErrCodeHT.Add("551", "用户非本地，请尝试<forward-path>");
            ErrCodeHT.Add("452", "系统存储不足，要求的操作未执行");
            ErrCodeHT.Add("552", "过量的存储分配，要求的操作未执行");
            ErrCodeHT.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）");
            ErrCodeHT.Add("432", "需要一个密码转换");
            ErrCodeHT.Add("534", "认证机制过于简单");
            ErrCodeHT.Add("538", "当前请求的认证机制需要加密");
            ErrCodeHT.Add("454", "临时认证失败");
            ErrCodeHT.Add("530", "需要认证");

            RightCodeHT.Add("220", "服务就绪");
            RightCodeHT.Add("250", "要求的邮件操作完成");
            RightCodeHT.Add("251", "用户非本地，将转发向<forward-path>");
            RightCodeHT.Add("354", "开始邮件输入，以<CRLF>.<CRLF>结束");
            RightCodeHT.Add("221", "服务关闭传输信道");
            RightCodeHT.Add("334", "服务器响应验证Base64字符串");
            RightCodeHT.Add("235", "验证成功");
        }


        //用于分割附件的分割符.
        protected string boundary = "=====000_HuolxPubClass113273537350_=====";
        protected string boundary1 = "=====001_HuolxPubClass113273537350_=====";


        /// <summary>
        /// 用于存放附件路径的信息
        /// </summary>		
        protected ArrayList Attachments = new ArrayList();


        /// <summary>
        /// 添加一个附件,需使用绝对路径
        /// </summary>	
        public bool AddAttachment(string path)
        {
            if (File.Exists(path))
            {
                Attachments.Add(path);
                return true;
            }
            else
            {
                errmsg += "要附加的文件不存在" + enter;
                return false;
            }
        }

        /// <summary>
        /// 附件的BASE64编码字符串
        /// </summary>
        /// <param name="path">附件路径</param>
        protected string AttachmentB64Str(string path)
        {
            FileStream fs;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch
            {
                errmsg += "要附加的文件不存在" + enter;
                return Base64Encode("要附加的文件:" + path + "不存在");
            }
            int fl = (int) fs.Length;
            byte[] barray = new byte[fl];
            fs.Read(barray, 0, fl);
            fs.Close();
            return B64StrLine(Convert.ToBase64String(barray));
        }

        /// <summary>
        /// 如果文件名中含有非英文字母，则将其编码
        /// </summary>
        protected string AttachmentNameStr(string fn)
        {
            if (Encoding.Default.GetByteCount(fn) > fn.Length)
            {
                return "=?" + Charset.ToUpper() + "?B?" + Base64Encode(fn) + "?=";
            }
            else
            {
                return fn;
            }
        }

        protected string B64StrLine(string str)
        {
            StringBuilder B64sb = new StringBuilder(str);
            for (int i = 76; i < B64sb.Length; i += 78)
            {
                B64sb.Insert(i, enter);
            }
            return B64sb.ToString();
        }


        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        protected static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }


        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="dstr">要解码的字符串</param>
        protected static string Base64Decode(string dstr)
        {
            byte[] barray;
            barray = Convert.FromBase64String(dstr);
            return Encoding.Default.GetString(barray);
        }

        public CEMailData()
        {
        }

        ~CEMailData()
        {
            if (ns != null)
                ns.Close();
            if (tc != null)
                tc.Close();
        }

        /// <summary>
        /// 收件人姓名
        /// </summary>	
        public string RecipientName = "";


        /// <summary>
        /// 添加一个收件人
        /// </summary>	
        /// <param name="str">收件人地址</param>
        /// <param name="ra"></param>
        /// <returns></returns>
        protected bool AddRs(string str, ArrayList ra)
        {
            str = str.Trim();

            if (str == null || str == "" || str.IndexOf("@") == -1)
            {
                return true;
                //				上面的语句自动滤除无效的收件人，为了不影响正常运作，未返回错误，如果您需要严格的检查收件人，请替换为下面的语句。
                //				errmsg+="存在无效收件人：" +str;
                //				return false;
            }

            if (ra.Count < RecipientMaxNum)
            {
                ra.Add(str);
                return true;
            }
            else
            {
                errmsg += "收件人过多";
                return false;
            }
        }


        /// <summary>
        /// 添加一组收件人（不超过10个），参数为字符串数组
        /// </summary>
        /// <param name="str">保存有收件人地址的字符串数组（不超过10个）</param>	
        /// <param name="ra"></param>
        /// <returns></returns>
        protected bool AddRs(string[] str, ArrayList ra)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!AddRs(str[i], ra))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 添加一个收件人
        /// </summary>	
        /// <param name="str">收件人地址</param>
        public bool AddRecipient(string str)
        {
            return AddRs(str, Recipient);
        }


        /// <summary>
        /// 添加一组收件人（不超过10个），参数为字符串数组
        /// </summary>
        /// <param name="str">保存有收件人地址的字符串数组（不超过RecipientMaxNum个）</param>	
        public bool AddRecipient(string[] str)
        {
            return AddRs(str, Recipient);
        }


        /// <summary>
        /// 添加一个抄送收件人
        /// </summary>
        /// <param name="str">收件人地址</param>
        public bool AddRecipientCC(string str)
        {
            return AddRs(str, RecipientCC);
        }


        /// <summary>
        /// 添加一组抄送收件人（不超过10个），参数为字符串数组
        /// </summary>	
        /// <param name="str">保存有收件人地址的字符串数组（不超过RecipientMaxNum个）</param>
        public bool AddRecipientCC(string[] str)
        {
            return AddRs(str, RecipientCC);
        }


        /// <summary>
        /// 添加一个密件收件人
        /// </summary>
        /// <param name="str">收件人地址</param>
        public bool AddRecipientBCC(string str)
        {
            return AddRs(str, RecipientBCC);
        }


        /// <summary>
        /// 添加一组密件收件人（不超过10个），参数为字符串数组
        /// </summary>	
        /// <param name="str">保存有收件人地址的字符串数组（不超过RecipientMaxNum个）</param>
        public bool AddRecipientBCC(string[] str)
        {
            return AddRs(str, RecipientBCC);
        }


        /// <summary>
        /// 发送SMTP命令
        /// </summary>	
        protected bool SendCommand(string Command)
        {
            byte[] WriteBuffer;
            if (Command == null || Command.Trim() == "")
            {
                return true;
            }
            logs += Command;
            WriteBuffer = Encoding.Default.GetBytes(Command);
            try
            {
                ns.Write(WriteBuffer, 0, WriteBuffer.Length);
            }
            catch
            {
                errmsg = "网络连接错误";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接收SMTP服务器回应
        /// </summary>
        protected string RecvResponse()
        {
            int StreamSize;
            string ReturnValue = "false";
            byte[] ReadBuffer = new byte[4096];

            try
            {
                StreamSize = ns.Read(ReadBuffer, 0, ReadBuffer.Length);
            }
            catch
            {
                errmsg = "网络连接错误";
                return ReturnValue;
            }

            if (StreamSize == 0)
            {
                return ReturnValue;
            }
            else
            {
                ReturnValue = Encoding.Default.GetString(ReadBuffer).Substring(0, StreamSize).Trim();
                logs += ReturnValue;
                return ReturnValue;
            }
        }


        /// <summary>
        /// 与服务器交互，发送一条命令并接收回应。
        /// </summary>
        /// <param name="Command">一个要发送的命令</param>
        /// <param name="errstr">如果错误，要反馈的信息</param>
        protected bool Dialog(string Command, string errstr)
        {
            if (Command == null || Command.Trim() == "")
            {
                return true;
            }
            if (SendCommand(Command))
            {
                string RR = RecvResponse();
                if (RR == "false")
                {
                    return false;
                }
                string RRCode;

                if (RR.Length >= 3)
                    RRCode = RR.Substring(0, 3);
                else
                    RRCode = RR;

                if (ErrCodeHT[RRCode] != null)
                {
                    errmsg += (RRCode + ErrCodeHT[RRCode]);
                    errmsg += enter;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 与服务器交互，发送一组命令并接收回应。
        /// </summary>
        protected bool Dialog(ArrayList Command, string errstr)
        {
            foreach (String item in Command)
            {
                if (!Dialog(item, ""))
                {
                    errmsg += enter;
                    errmsg += errstr;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// SMTP验证过程.
        /// </summary>
        protected bool SmtpAuth()
        {
            ArrayList SendBuffer = new ArrayList();
            string SendBufferstr;
            SendBufferstr = "EHLO " + mailserver + enter;
            //			SendBufferstr="HELO " + mailserver + enter;
            //这个地方经常出现命令错位，不得以加入特殊控制代码，才能正常执行。
            //以后最好能有更好的解决办法。
            if (SendCommand(SendBufferstr))
            {
                int i = 0;
                while (true)
                {
                    if (ns.DataAvailable)
                    {
                        string RR = RecvResponse();
                        if (RR == "false")
                        {
                            return false;
                        }
                        string RRCode = RR.Substring(0, 3);
                        if (RightCodeHT[RRCode] != null)
                        {
                            if (RR.IndexOf("AUTH") != -1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (ErrCodeHT[RRCode] != null)
                            {
                                errmsg += (RRCode + ErrCodeHT[RRCode]);
                                errmsg += enter;
                                errmsg += "发送EHLO命令出错，服务器可能不需要验证" + enter;
                            }
                            else
                            {
                                errmsg += RR;
                                errmsg += "发送EHLO命令出错，不明错误,请与作者联系" + enter;
                            }
                            return false;
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                        i++;
                        if (i > 6)
                        {
                            errmsg += "收不到AUTH指令，可能是连接超时，或者服务器根本不需要验证" + enter;
                            return false;
                        }
                    }
                }
            }
            else
            {
                errmsg += "发送ehlo命令失败";
                return false;
            }

            SendBuffer.Add("AUTH LOGIN" + enter);
            SendBuffer.Add(Base64Encode(username) + enter);
            SendBuffer.Add(Base64Encode(password) + enter);
            if (!Dialog(SendBuffer, "SMTP服务器验证失败，请核对用户名和密码。"))
                return false;
            return true;
        }


        protected bool SendEmail()
        {
            //连接网络
            try
            {
                tc = new TcpClient(mailserver, mailserverport);
            }
            catch (Exception e)
            {
                errmsg = e.ToString();
                return false;
            }

            ns = tc.GetStream();
            SMTPCodeAdd();

            //验证网络连接是否正确
            if (RightCodeHT[RecvResponse().Substring(0, 3)] == null)
            {
                errmsg = "网络连接失败";
                return false;
            }


            ArrayList SendBuffer = new ArrayList();
            string SendBufferstr;

            //进行SMTP验证
            if (ESmtp)
            {
                if (!SmtpAuth())
                    return false;
            }
            else
            {
                SendBufferstr = "HELO " + mailserver + enter;
                if (!Dialog(SendBufferstr, ""))
                    return false;
            }

            //发件人信息
            SendBufferstr = "MAIL FROM:<" + From + ">" + enter;
            if (!Dialog(SendBufferstr, "发件人地址错误，或不能为空"))
                return false;

            //收件人列表
            SendBuffer.Clear();
            foreach (String item in Recipient)
            {
                SendBuffer.Add("RCPT TO:<" + item + ">" + enter);
            }
            if (!Dialog(SendBuffer, "收件人地址有误"))
                return false;


            //开始发送信件内容
            SendBufferstr = "DATA" + enter;
            if (!Dialog(SendBufferstr, ""))
                return false;

            //发件人
            SendBufferstr = "From:\"" + FromName + "\" <" + From + ">" + enter;

            //收件人
            SendBufferstr += "To:\"" + RecipientName + "\" <" + RecipientName + ">" + enter;

            //回复地址
            if (ReplyTo.Trim() != "")
            {
                SendBufferstr += "Reply-To: " + ReplyTo + enter;
            }

            //抄送收件人列表
            if (RecipientCC.Count > 0)
            {
                SendBufferstr += "CC:";
                foreach (String item in RecipientCC)
                {
                    SendBufferstr += item + "<" + item + ">," + enter;
                }
                SendBufferstr = SendBufferstr.Substring(0, SendBufferstr.Length - 3) + enter;
            }

            //密件收件人列表
            if (RecipientBCC.Count > 0)
            {
                SendBufferstr += "BCC:";
                foreach (String item in RecipientBCC)
                {
                    SendBufferstr += item + "<" + item + ">," + enter;
                }
                SendBufferstr = SendBufferstr.Substring(0, SendBufferstr.Length - 3) + enter;
            }

            //邮件主题
            if (Charset == "")
            {
                SendBufferstr += "Subject:" + Subject + enter;
            }
            else
            {
                SendBufferstr += "Subject:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) + "?=" + enter;
            }

            //是否需要收件人发送收条
            if (ReturnReceipt)
            {
                SendBufferstr += "Disposition-Notification-To: \"" + FromName + "\" <" + ReplyTo + ">" + enter;
            }

            SendBufferstr += "X-Priority:" + priority + enter;
            SendBufferstr += "X-MSMail-Priority:" + priority + enter;
            SendBufferstr += "Importance:" + priority + enter;
            SendBufferstr += "X-Mailer: Huolx.Pubclass" + enter;
            SendBufferstr += "MIME-Version: 1.0" + enter;

            //如果有附件,发送分割信息
            if (Attachments.Count > 0)
            {
                SendBufferstr += "Content-Type: multipart/mixed;" + enter;
                SendBufferstr += "	boundary=\"" + boundary + "\"" + enter + enter;
                SendBufferstr += "This is a multi-part message in MIME format." + enter + enter;
                SendBufferstr += "--" + boundary + enter;
                SendBufferstr += "Content-Type: multipart/alternative;" + enter;
                SendBufferstr += "	boundary=\"" + boundary1 + "\"" + enter + enter + enter;
                SendBufferstr += "--" + boundary1 + enter;
            }


            //判断信件格式是否html
            if (Html)
            {
                SendBufferstr += "Content-Type: text/html;" + enter;
            }
            else
            {
                SendBufferstr += "Content-Type: text/plain;" + enter;
            }

            //编码信息
            if (Charset == "")
            {
                SendBufferstr += "	charset=\"iso-8859-1\"" + enter;
            }
            else
            {
                SendBufferstr += "	charset=\"" + Charset.ToLower() + "\"" + enter;
            }

            SendBufferstr += "Content-Transfer-Encoding: base64" + enter;

            SendBufferstr += enter + enter;
            SendBufferstr += B64StrLine(Base64Encode(Body)) + enter;

            //如果有附件,开始发送附件.
            if (Attachments.Count > 0)
            {
                SendBufferstr += enter + "--" + boundary1 + "--" + enter + enter;
                foreach (String item in Attachments)
                {
                    SendBufferstr += "--" + boundary + enter;
                    SendBufferstr += "Content-Type: application/octet-stream;" + enter;
                    SendBufferstr += "	name=\"" + AttachmentNameStr(item.Substring(item.LastIndexOf("\\") + 1)) + "\"" + enter;
                    SendBufferstr += "Content-Transfer-Encoding: base64" + enter;
                    SendBufferstr += "Content-Disposition: attachment;" + enter;
                    SendBufferstr += "	filename=\"" + AttachmentNameStr(item.Substring(item.LastIndexOf("\\") + 1)) + "\"" + enter + enter;
                    SendBufferstr += AttachmentB64Str(item) + enter + enter;
                }
                SendBufferstr += "--" + boundary + "--" + enter + enter;
            }
            SendBufferstr += enter + "." + enter;

            if (!Dialog(SendBufferstr, "错误信件信息"))
                return false;


            SendBufferstr = "QUIT" + enter;
            if (!Dialog(SendBufferstr, "断开连接时错误"))
                return false;


            ns.Close();
            tc.Close();
            return true;
        }


        /// <summary>
        /// 发送邮件方法，所有参数均通过属性设置。
        /// </summary>
        public bool Send()
        {
            if (Recipient.Count == 0)
            {
                errmsg = "收件人列表不能为空";
                return false;
            }


            if (RecipientName == "")
                RecipientName = Recipient[0].ToString();

            if (mailserver.Trim() == "")
            {
                errmsg = "必须指定SMTP服务器";
                return false;
            }

            return SendEmail();
        }

        public void ClearRecipient()
        {
            Recipient.Clear();
        }


        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="smtpserver">smtp服务器信息，如"username:password@www.smtpserver.com:25"，也可去掉部分次要信息，如"www.smtpserver.com"</param>
        public bool Send(string smtpserver)
        {
            MailDomain = smtpserver;
            return Send();
        }


        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="smtpserver">smtp服务器信息，如"username:password@www.smtpserver.com:25"，也可去掉部分次要信息，如"www.smtpserver.com"</param>
        /// <param name="from">发件人mail地址</param>
        /// <param name="fromname">发件人姓名</param>
        /// <param name="replyto">回复邮件地址</param>
        /// <param name="to">收件人地址</param>
        /// <param name="toname">收件人姓名</param>
        /// <param name="html">是否HTML邮件</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        public bool Send(string smtpserver, string from, string fromname, string replyto, string to, string toname, bool html, string subject, string body)
        {
            MailDomain = smtpserver;
            From = from;
            FromName = fromname;
            ReplyTo = replyto;
            AddRecipient(to);
            RecipientName = toname;
            Html = html;
            Subject = subject;
            Body = body;
            return Send();
        }
    }
}