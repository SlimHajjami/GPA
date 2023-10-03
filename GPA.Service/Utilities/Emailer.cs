using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GPA.Service.Utilities
{
    public class Emailer
    {
        #region Properties

        public string SmtpHost
        {
            get { return _SmtpHost; }
            set { _SmtpHost = value; }
        }

        public int SmtpPort
        {
            get { return _SmtpPort; }
            set { _SmtpPort = value; }
        }

        public bool EnableSsl
        {
            get { return _EnableSsl; }
            set { _EnableSsl = value; }
        }

        public string SmtpUser
        {
            get { return _SmtpUser; }
            set { _SmtpUser = value; }
        }

        public string SmtpUserPassword
        {
            get { return _SmtpUserPassword; }
            set { _SmtpUserPassword = value; }
        }

        public bool IsBodyHtml
        {
            get { return _IsBodyHtml; }
            set { _IsBodyHtml = value; }
        }

        public string FromEmail
        {
            get { return _FromEmail; }
            set { _FromEmail = value; }
        }

        public string FromName
        {
            get { return _FromName; }
            set { _FromName = value; }
        }

        #endregion

        private string _SmtpHost = string.Empty;
        private int _SmtpPort = 0;
        private bool _EnableSsl = false;
        private string _SmtpUser = string.Empty;
        private string _SmtpUserPassword = string.Empty;
        private bool _IsBodyHtml = false;
        private string _FromEmail = string.Empty;
        private string _FromName = string.Empty;

        public Emailer() { }

        public Emailer(string strSmtpHost, string strSmtpUser, string strSmtpUserPassword, string strFromEmail, string strFromName)
        {
            SmtpHost = strSmtpHost;
            SmtpUser = strSmtpUser;
            SmtpUserPassword = strSmtpUserPassword;
            FromEmail = strFromEmail;
            FromName = strFromName;
        }

        public void SendEmail(string strSubject, List<string> ListTo, List<string> ListCc, string strBody, out string error)
        {
            error = "";
            System.Net.Mail.SmtpClient objSmtpClient = new System.Net.Mail.SmtpClient();
            objSmtpClient.Host = this.SmtpHost;
            objSmtpClient.Port = this.SmtpPort;
            objSmtpClient.EnableSsl = this.EnableSsl;

            objSmtpClient.Credentials = new System.Net.NetworkCredential(this.SmtpUser, this.SmtpUserPassword);

            System.Net.Mail.MailMessage objMailMessage = new System.Net.Mail.MailMessage();
            objMailMessage.From = new System.Net.Mail.MailAddress(this.FromEmail, this.FromName);
            objMailMessage.IsBodyHtml = this.IsBodyHtml;

            objMailMessage.Subject = strSubject;
            objMailMessage.Body = strBody;

            for (int i = 0; i < ListTo.Count; i++)
            {
                objMailMessage.To.Add(ListTo[i]);
            }

            for (int i = 0; i < ListCc.Count; i++)
            {
                objMailMessage.CC.Add(ListCc[i]);
            }

            try
            {
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
