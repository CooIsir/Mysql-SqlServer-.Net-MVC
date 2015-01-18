using System;

namespace com.Utility
{
	/// <summary>
	/// CEmailSend ��ժҪ˵����
	/// </summary>
	public class CEmailSend
	{
        /// <summary>
        /// �趨����������
        /// </summary>
        public string FromName = "39������";
        /// <summary>
		/// �趨�ʼ�����
		/// </summary>
		public string Subject = "";
		/// <summary>
		/// �趨�ʼ�����
		/// </summary>
		public string Body = "";
		/// <summary>
		/// Ҫ���͵�����
		/// </summary>
		public string SendTo = "";

        public string ErrorMsg = "";

	    public bool send()
		{
            CEMailData ESM = new CEMailData();

            ESM.From = "hd@mail.39.net";				//�趨�����˵�ַ(������д)
            ESM.FromName = FromName;			    //�趨����������

            ESM.Html = true;

            ESM.Subject = Subject;					//�趨�ʼ�����
            ESM.Body = Body;						//�趨�ʼ�����
            ESM.ReplyTo = "hd@mail.39.net";			//�ظ������ַ 

            ESM.MailDomain = "mail.39.net";		//�ʼ���������ַ
            ESM.MailDomainPort = 25;            //�ʼ��������˿�
            //ESM.MailServerUserName = @"mail39\hd";		    //�趨SMTP��֤���û���
            //ESM.MailServerPassWord = "666666";			//�趨SMTP��֤������

            ESM.AddRecipient(SendTo);				//���Ҫ���͵�����

            //CDONTS.NewMailClass nmail = new CDONTS.NewMailClass();
            //nmail.BodyFormat = 0;		// 0:html��ʽ, 1:text	
            //nmail.SetLocaleIDs(936);
            //nmail.MailFormat = 0;		// 0:html��ʽ  1:text

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
