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
        /// �趨���Դ��룬Ĭ���趨ΪGB2312���粻��Ҫ������Ϊ""
        /// </summary>
        public string Charset = "GB2312";

        /// <summary>
        /// �����˵�ַ��Ĭ��ֵ��ggwscdc@163.com
        /// </summary>
        public string From = "ggwscdc@163.com";

        /// <summary>
        /// ���������� Ĭ��ֵ��39������
        /// </summary>
        public string FromName = "��������";

        /// <summary>
        /// �ظ��ʼ���ַ��Ĭ��ֵ: ggwscdc@163.com
        /// </summary>
        public string ReplyTo = "ggwscdc@163.com";

        /// <summary>
        /// �ʼ�����������
        /// </summary>	
        protected string mailserver;

        /// <summary>
        /// �ʼ���������������֤��Ϣ
        /// ���磺"user:pass@www.server.com:25"��Ҳ��ʡ�Դ�Ҫ��Ϣ����"user:pass@www.server.com"��"www.server.com"
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
        /// �ʼ��������˿ں�
        /// </summary>	
        protected int mailserverport = 25;

        /// <summary>
        /// �ʼ��������˿ں�
        /// </summary>	
        public int MailDomainPort
        {
            set { mailserverport = value; }
        }


        /// <summary>
        /// �Ƿ���ҪSMTP��֤
        /// </summary>		
        protected bool ESmtp = false;

        /// <summary>
        /// SMTP��֤ʱʹ�õ��û���
        /// </summary>
        protected string username = "";

        /// <summary>
        /// SMTP��֤ʱʹ�õ��û���
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
        /// SMTP��֤ʱʹ�õ�����
        /// </summary>
        protected string password = "";

        /// <summary>
        /// SMTP��֤ʱʹ�õ�����
        /// </summary>
        public string MailServerPassWord
        {
            set { password = value; }
        }

        /// <summary>
        /// �ʼ�����
        /// </summary>		
        public string Subject = "";

        /// <summary>
        /// �Ƿ�Html�ʼ�
        /// </summary>		
        public bool Html = false;

        /// <summary>
        /// �ռ����Ƿ�������
        /// </summary>
        public bool ReturnReceipt = false;

        /// <summary>
        /// �ʼ�����
        /// </summary>		
        public string Body = "";


        /// <summary>
        /// �ռ���������������ںܶ�SMTP�������ռ��˵�����������Է�ֹ����ʼ����ģ��������һ�㶼������10�����¡�
        /// </summary>
        protected int RecipientMaxNum = 10;

        /// <summary>
        /// �ռ����б�
        /// </summary>
        protected ArrayList Recipient = new ArrayList();

        /// <summary>
        ///�����ռ����б�
        /// </summary>
        protected ArrayList RecipientCC = new ArrayList();

        /// <summary>
        /// �����ռ����б�
        /// </summary>
        protected ArrayList RecipientBCC = new ArrayList();

        /// <summary>
        /// �ʼ��������ȼ���������Ϊ"High","Normal","Low"��"1","3","5"
        /// </summary>
        protected string priority = "Normal";

        /// <summary>
        /// �ʼ��������ȼ���������Ϊ"High","Normal","Low"��"1","3","5"
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
        /// ������Ϣ����
        /// </summary>
        protected string errmsg;

        /// <summary>
        /// ������Ϣ����
        /// </summary>		
        public string ErrorMessage
        {
            get { return errmsg; }
        }


        /// <summary>
        /// ������������¼
        /// </summary>
        protected string logs = "";

        /// <summary>
        /// ������������¼���緢�ֱ��������ʹ�õ�SMTP���������뽫����ʱ��Logs�����ң�huolx@s2008.com�����ҽ��������ԭ��
        /// </summary>
        public string Logs
        {
            get { return logs; }
        }

        /// <summary>
        /// ������������¼(html��ʽ)���緢�ֱ��������ʹ�õ�SMTP���������뽫����ʱ��Logs�����ң�huolx@s2008.com�����ҽ��������ԭ��
        /// </summary>
        public string HtmlLogs
        {
            get { return logs.Replace("<", "&lt;").Replace(">", "&gt;").Replace(enter, "<br>").Replace(" ", "&nbsp;"); }
        }

        protected string enter = "\r\n";

        /// <summary>
        /// TcpClient�����������ӷ�����
        /// </summary>	
        protected TcpClient tc;

        /// <summary>
        /// NetworkStream����
        /// </summary>	
        protected NetworkStream ns;

        /// <summary>
        /// SMTP��������ϣ��
        /// </summary>
        protected Hashtable ErrCodeHT = new Hashtable();

        /// <summary>
        /// SMTP��ȷ�����ϣ��
        /// </summary>
        protected Hashtable RightCodeHT = new Hashtable();

        /// <summary>
        /// SMTP��Ӧ�����ϣ��
        /// </summary>
        protected void SMTPCodeAdd()
        {
            ErrCodeHT.Clear();
            RightCodeHT.Clear();
            ErrCodeHT.Add("500", "�����ַ����");
            ErrCodeHT.Add("501", "������ʽ����");
            ErrCodeHT.Add("502", "�����ʵ��");
            ErrCodeHT.Add("503", "��������ҪSMTP��֤");
            ErrCodeHT.Add("504", "�����������ʵ��");
            ErrCodeHT.Add("421", "����δ�������رմ����ŵ�");
            ErrCodeHT.Add("450", "Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����æ��");
            ErrCodeHT.Add("550", "Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����δ�ҵ����򲻿ɷ��ʣ�");
            ErrCodeHT.Add("451", "����Ҫ��Ĳ�������������г���");
            ErrCodeHT.Add("551", "�û��Ǳ��أ��볢��<forward-path>");
            ErrCodeHT.Add("452", "ϵͳ�洢���㣬Ҫ��Ĳ���δִ��");
            ErrCodeHT.Add("552", "�����Ĵ洢���䣬Ҫ��Ĳ���δִ��");
            ErrCodeHT.Add("553", "�����������ã�Ҫ��Ĳ���δִ�У����������ʽ����");
            ErrCodeHT.Add("432", "��Ҫһ������ת��");
            ErrCodeHT.Add("534", "��֤���ƹ��ڼ�");
            ErrCodeHT.Add("538", "��ǰ�������֤������Ҫ����");
            ErrCodeHT.Add("454", "��ʱ��֤ʧ��");
            ErrCodeHT.Add("530", "��Ҫ��֤");

            RightCodeHT.Add("220", "�������");
            RightCodeHT.Add("250", "Ҫ����ʼ��������");
            RightCodeHT.Add("251", "�û��Ǳ��أ���ת����<forward-path>");
            RightCodeHT.Add("354", "��ʼ�ʼ����룬��<CRLF>.<CRLF>����");
            RightCodeHT.Add("221", "����رմ����ŵ�");
            RightCodeHT.Add("334", "��������Ӧ��֤Base64�ַ���");
            RightCodeHT.Add("235", "��֤�ɹ�");
        }


        //���ڷָ���ķָ��.
        protected string boundary = "=====000_HuolxPubClass113273537350_=====";
        protected string boundary1 = "=====001_HuolxPubClass113273537350_=====";


        /// <summary>
        /// ���ڴ�Ÿ���·������Ϣ
        /// </summary>		
        protected ArrayList Attachments = new ArrayList();


        /// <summary>
        /// ���һ������,��ʹ�þ���·��
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
                errmsg += "Ҫ���ӵ��ļ�������" + enter;
                return false;
            }
        }

        /// <summary>
        /// ������BASE64�����ַ���
        /// </summary>
        /// <param name="path">����·��</param>
        protected string AttachmentB64Str(string path)
        {
            FileStream fs;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch
            {
                errmsg += "Ҫ���ӵ��ļ�������" + enter;
                return Base64Encode("Ҫ���ӵ��ļ�:" + path + "������");
            }
            int fl = (int) fs.Length;
            byte[] barray = new byte[fl];
            fs.Read(barray, 0, fl);
            fs.Close();
            return B64StrLine(Convert.ToBase64String(barray));
        }

        /// <summary>
        /// ����ļ����к��з�Ӣ����ĸ���������
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
        /// ���ַ�������ΪBase64�ַ���
        /// </summary>
        /// <param name="str">Ҫ������ַ���</param>
        protected static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }


        /// <summary>
        /// ��Base64�ַ�������Ϊ��ͨ�ַ���
        /// </summary>
        /// <param name="dstr">Ҫ������ַ���</param>
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
        /// �ռ�������
        /// </summary>	
        public string RecipientName = "";


        /// <summary>
        /// ���һ���ռ���
        /// </summary>	
        /// <param name="str">�ռ��˵�ַ</param>
        /// <param name="ra"></param>
        /// <returns></returns>
        protected bool AddRs(string str, ArrayList ra)
        {
            str = str.Trim();

            if (str == null || str == "" || str.IndexOf("@") == -1)
            {
                return true;
                //				���������Զ��˳���Ч���ռ��ˣ�Ϊ�˲�Ӱ������������δ���ش����������Ҫ�ϸ�ļ���ռ��ˣ����滻Ϊ�������䡣
                //				errmsg+="������Ч�ռ��ˣ�" +str;
                //				return false;
            }

            if (ra.Count < RecipientMaxNum)
            {
                ra.Add(str);
                return true;
            }
            else
            {
                errmsg += "�ռ��˹���";
                return false;
            }
        }


        /// <summary>
        /// ���һ���ռ��ˣ�������10����������Ϊ�ַ�������
        /// </summary>
        /// <param name="str">�������ռ��˵�ַ���ַ������飨������10����</param>	
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
        /// ���һ���ռ���
        /// </summary>	
        /// <param name="str">�ռ��˵�ַ</param>
        public bool AddRecipient(string str)
        {
            return AddRs(str, Recipient);
        }


        /// <summary>
        /// ���һ���ռ��ˣ�������10����������Ϊ�ַ�������
        /// </summary>
        /// <param name="str">�������ռ��˵�ַ���ַ������飨������RecipientMaxNum����</param>	
        public bool AddRecipient(string[] str)
        {
            return AddRs(str, Recipient);
        }


        /// <summary>
        /// ���һ�������ռ���
        /// </summary>
        /// <param name="str">�ռ��˵�ַ</param>
        public bool AddRecipientCC(string str)
        {
            return AddRs(str, RecipientCC);
        }


        /// <summary>
        /// ���һ�鳭���ռ��ˣ�������10����������Ϊ�ַ�������
        /// </summary>	
        /// <param name="str">�������ռ��˵�ַ���ַ������飨������RecipientMaxNum����</param>
        public bool AddRecipientCC(string[] str)
        {
            return AddRs(str, RecipientCC);
        }


        /// <summary>
        /// ���һ���ܼ��ռ���
        /// </summary>
        /// <param name="str">�ռ��˵�ַ</param>
        public bool AddRecipientBCC(string str)
        {
            return AddRs(str, RecipientBCC);
        }


        /// <summary>
        /// ���һ���ܼ��ռ��ˣ�������10����������Ϊ�ַ�������
        /// </summary>	
        /// <param name="str">�������ռ��˵�ַ���ַ������飨������RecipientMaxNum����</param>
        public bool AddRecipientBCC(string[] str)
        {
            return AddRs(str, RecipientBCC);
        }


        /// <summary>
        /// ����SMTP����
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
                errmsg = "�������Ӵ���";
                return false;
            }
            return true;
        }

        /// <summary>
        /// ����SMTP��������Ӧ
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
                errmsg = "�������Ӵ���";
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
        /// �����������������һ��������ջ�Ӧ��
        /// </summary>
        /// <param name="Command">һ��Ҫ���͵�����</param>
        /// <param name="errstr">�������Ҫ��������Ϣ</param>
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
        /// �����������������һ��������ջ�Ӧ��
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
        /// SMTP��֤����.
        /// </summary>
        protected bool SmtpAuth()
        {
            ArrayList SendBuffer = new ArrayList();
            string SendBufferstr;
            SendBufferstr = "EHLO " + mailserver + enter;
            //			SendBufferstr="HELO " + mailserver + enter;
            //����ط��������������λ�������Լ���������ƴ��룬��������ִ�С�
            //�Ժ�������и��õĽ���취��
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
                                errmsg += "����EHLO����������������ܲ���Ҫ��֤" + enter;
                            }
                            else
                            {
                                errmsg += RR;
                                errmsg += "����EHLO���������������,����������ϵ" + enter;
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
                            errmsg += "�ղ���AUTHָ����������ӳ�ʱ�����߷�������������Ҫ��֤" + enter;
                            return false;
                        }
                    }
                }
            }
            else
            {
                errmsg += "����ehlo����ʧ��";
                return false;
            }

            SendBuffer.Add("AUTH LOGIN" + enter);
            SendBuffer.Add(Base64Encode(username) + enter);
            SendBuffer.Add(Base64Encode(password) + enter);
            if (!Dialog(SendBuffer, "SMTP��������֤ʧ�ܣ���˶��û��������롣"))
                return false;
            return true;
        }


        protected bool SendEmail()
        {
            //��������
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

            //��֤���������Ƿ���ȷ
            if (RightCodeHT[RecvResponse().Substring(0, 3)] == null)
            {
                errmsg = "��������ʧ��";
                return false;
            }


            ArrayList SendBuffer = new ArrayList();
            string SendBufferstr;

            //����SMTP��֤
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

            //��������Ϣ
            SendBufferstr = "MAIL FROM:<" + From + ">" + enter;
            if (!Dialog(SendBufferstr, "�����˵�ַ���󣬻���Ϊ��"))
                return false;

            //�ռ����б�
            SendBuffer.Clear();
            foreach (String item in Recipient)
            {
                SendBuffer.Add("RCPT TO:<" + item + ">" + enter);
            }
            if (!Dialog(SendBuffer, "�ռ��˵�ַ����"))
                return false;


            //��ʼ�����ż�����
            SendBufferstr = "DATA" + enter;
            if (!Dialog(SendBufferstr, ""))
                return false;

            //������
            SendBufferstr = "From:\"" + FromName + "\" <" + From + ">" + enter;

            //�ռ���
            SendBufferstr += "To:\"" + RecipientName + "\" <" + RecipientName + ">" + enter;

            //�ظ���ַ
            if (ReplyTo.Trim() != "")
            {
                SendBufferstr += "Reply-To: " + ReplyTo + enter;
            }

            //�����ռ����б�
            if (RecipientCC.Count > 0)
            {
                SendBufferstr += "CC:";
                foreach (String item in RecipientCC)
                {
                    SendBufferstr += item + "<" + item + ">," + enter;
                }
                SendBufferstr = SendBufferstr.Substring(0, SendBufferstr.Length - 3) + enter;
            }

            //�ܼ��ռ����б�
            if (RecipientBCC.Count > 0)
            {
                SendBufferstr += "BCC:";
                foreach (String item in RecipientBCC)
                {
                    SendBufferstr += item + "<" + item + ">," + enter;
                }
                SendBufferstr = SendBufferstr.Substring(0, SendBufferstr.Length - 3) + enter;
            }

            //�ʼ�����
            if (Charset == "")
            {
                SendBufferstr += "Subject:" + Subject + enter;
            }
            else
            {
                SendBufferstr += "Subject:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) + "?=" + enter;
            }

            //�Ƿ���Ҫ�ռ��˷�������
            if (ReturnReceipt)
            {
                SendBufferstr += "Disposition-Notification-To: \"" + FromName + "\" <" + ReplyTo + ">" + enter;
            }

            SendBufferstr += "X-Priority:" + priority + enter;
            SendBufferstr += "X-MSMail-Priority:" + priority + enter;
            SendBufferstr += "Importance:" + priority + enter;
            SendBufferstr += "X-Mailer: Huolx.Pubclass" + enter;
            SendBufferstr += "MIME-Version: 1.0" + enter;

            //����и���,���ͷָ���Ϣ
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


            //�ж��ż���ʽ�Ƿ�html
            if (Html)
            {
                SendBufferstr += "Content-Type: text/html;" + enter;
            }
            else
            {
                SendBufferstr += "Content-Type: text/plain;" + enter;
            }

            //������Ϣ
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

            //����и���,��ʼ���͸���.
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

            if (!Dialog(SendBufferstr, "�����ż���Ϣ"))
                return false;


            SendBufferstr = "QUIT" + enter;
            if (!Dialog(SendBufferstr, "�Ͽ�����ʱ����"))
                return false;


            ns.Close();
            tc.Close();
            return true;
        }


        /// <summary>
        /// �����ʼ����������в�����ͨ���������á�
        /// </summary>
        public bool Send()
        {
            if (Recipient.Count == 0)
            {
                errmsg = "�ռ����б���Ϊ��";
                return false;
            }


            if (RecipientName == "")
                RecipientName = Recipient[0].ToString();

            if (mailserver.Trim() == "")
            {
                errmsg = "����ָ��SMTP������";
                return false;
            }

            return SendEmail();
        }

        public void ClearRecipient()
        {
            Recipient.Clear();
        }


        /// <summary>
        /// �����ʼ�����
        /// </summary>
        /// <param name="smtpserver">smtp��������Ϣ����"username:password@www.smtpserver.com:25"��Ҳ��ȥ�����ִ�Ҫ��Ϣ����"www.smtpserver.com"</param>
        public bool Send(string smtpserver)
        {
            MailDomain = smtpserver;
            return Send();
        }


        /// <summary>
        /// �����ʼ�����
        /// </summary>
        /// <param name="smtpserver">smtp��������Ϣ����"username:password@www.smtpserver.com:25"��Ҳ��ȥ�����ִ�Ҫ��Ϣ����"www.smtpserver.com"</param>
        /// <param name="from">������mail��ַ</param>
        /// <param name="fromname">����������</param>
        /// <param name="replyto">�ظ��ʼ���ַ</param>
        /// <param name="to">�ռ��˵�ַ</param>
        /// <param name="toname">�ռ�������</param>
        /// <param name="html">�Ƿ�HTML�ʼ�</param>
        /// <param name="subject">�ʼ�����</param>
        /// <param name="body">�ʼ�����</param>
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