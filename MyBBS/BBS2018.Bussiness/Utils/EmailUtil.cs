using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Mail;
using System.Net;

namespace BBS2018.Bussiness.Utils
{
    public class EmailUtil
    {

        public MailMessage EmailMessage { get; set; }

        public SmtpClient EmailSmtpClient { get; set; }

        public int SendPort { get; set; }

        public string SenderServerHost { get; set; }

        public string SenderPassword { get; set; }

        public string SenderUserName { get; set; }

        public bool EnableSSL { get; set; }

        public bool EnablePwdAuthentication { get; set; }

        ///<summary>
        /// 构造函数
        ///</summary>
        ///<param name="server">发件箱的邮件服务器地址</param>
        ///<param name="toMail">收件人地址（可以是多个收件人，程序中是以“;"进行区分的）</param>
        ///<param name="fromMail">发件人地址</param>
        ///<param name="subject">邮件标题</param>
        ///<param name="emailBody">邮件内容（可以以html格式进行设计）</param>
        ///<param name="username">发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）</param>
        ///<param name="password">发件人邮箱密码</param>
        ///<param name="port">发送邮件所用的端口号（htmp协议默认为25）</param>
        ///<param name="sslEnable">true表示对邮件内容进行socket层加密传输，false表示不加密</param>
        ///<param name="pwdCheckEnable">true表示对发件人邮箱进行密码验证，false表示不对发件人邮箱进行密码验证</param>
        public EmailUtil(string server, string toMail, string fromMail, string subject, string emailBody, string userName, string password, int port, bool sslEnable, bool pwdCheckEnable)
        {
            try
            {
                this.EmailMessage = new MailMessage();
                this.EmailMessage.To.Add(toMail);
                this.EmailMessage.From = new MailAddress(fromMail);
                this.EmailMessage.Subject = subject;
                this.EmailMessage.Body = emailBody;
                this.EmailMessage.IsBodyHtml = true;
                this.EmailMessage.BodyEncoding = Encoding.UTF8;
                this.EmailMessage.Priority = MailPriority.Normal;
                this.SenderServerHost = server;
                this.SenderUserName = userName;
                this.SenderPassword = password;
                this.SendPort = port;
                this.EnableSSL = sslEnable;
                this.EnablePwdAuthentication = pwdCheckEnable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendEmail()
        {
            try
            {
                if (this.EmailMessage == null) return false;

                this.EmailSmtpClient = new SmtpClient();
                this.EmailSmtpClient.Host = this.SenderServerHost;
                this.EmailSmtpClient.Port = this.SendPort;
                this.EmailSmtpClient.UseDefaultCredentials = false;
                this.EmailSmtpClient.EnableSsl = this.EnableSSL;

                if (this.EnablePwdAuthentication)
                {
                    NetworkCredential nc = new System.Net.NetworkCredential(this.SenderUserName, this.SenderPassword);
                    //mSmtpClient.Credentials = new System.Net.NetworkCredential(this.mSenderUsername, this.mSenderPassword);
                    //NTLM: Secure Password Authentication in Microsoft Outlook Express
                    this.EmailSmtpClient.Credentials = nc.GetCredential(this.EmailSmtpClient.Host, this.EmailSmtpClient.Port, "NTLM");
                }
                else
                {
                    this.EmailSmtpClient.Credentials = new NetworkCredential(this.SenderUserName, this.SenderPassword);
                }

                this.EmailSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                this.EmailSmtpClient.Send(this.EmailMessage);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
