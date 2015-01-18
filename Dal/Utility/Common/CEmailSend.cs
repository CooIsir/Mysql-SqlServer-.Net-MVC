using System;

namespace com.Utility
{
	/// <summary>
	/// CEmailSend 的摘要说明。
	/// </summary>
	public class CEmailSend
	{
        /// <summary>
        /// 设定发件人姓名
        /// </summary>
        public string FromName = "39健康网";
        /// <summary>
		/// 设定邮件主题
		/// </summary>
		public string Subject = "";
		/// <summary>
		/// 设定邮件正文
		/// </summary>
		public string Body = "";
		/// <summary>
		/// 要发送的邮箱
		/// </summary>
		public string SendTo = "";

        public string ErrorMsg = "";

	    public bool send()
		{
            CEMailData ESM = new CEMailData();

            ESM.From = "hd@mail.39.net";				//设定发件人地址(必须填写)
            ESM.FromName = FromName;			    //设定发件人姓名

            ESM.Html = true;

            ESM.Subject = Subject;					//设定邮件主题
            ESM.Body = Body;						//设定邮件正文
            ESM.ReplyTo = "hd@mail.39.net";			//回复邮箱地址 

            ESM.MailDomain = "mail.39.net";		//邮件服务器地址
            ESM.MailDomainPort = 25;            //邮件服务器端口
            //ESM.MailServerUserName = @"mail39\hd";		    //设定SMTP验证的用户名
            //ESM.MailServerPassWord = "666666";			//设定SMTP验证的密码

            ESM.AddRecipient(SendTo);				//添加要发送的邮箱

            //CDONTS.NewMailClass nmail = new CDONTS.NewMailClass();
            //nmail.BodyFormat = 0;		// 0:html格式, 1:text	
            //nmail.SetLocaleIDs(936);
            //nmail.MailFormat = 0;		// 0:html格式  1:text

            //int sImportance = 2;
            //nmail.Send(FromName, SendTo, Subject, Body, sImportance);
			try
			{
                if (ESM.Send())
                    return true;
                else
                {
                    ErrorMsg = ESM.ErrorMessage;
                    return false;
                }
			}
			catch(Exception ex)
			{
                System.Diagnostics.Debug.Write(ex.Message);
				return false ;
			}
		}
	}
}
