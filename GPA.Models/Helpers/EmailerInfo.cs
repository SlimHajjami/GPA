using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.Models.Helpers
{
    public static class EmailerInfo
    {
        private static string SmtpHost { get; set; }
        private static string SmtpUser { get; set; }
        private static string FromName { get; set; }
        private static string FromEmail { get; set; }
        private static string SmtpPassword { get; set; }
        private static int SmtpPort { get; set; }
        private static bool Ssl { get; set; }

        public static string GetSmtpHost()
        {
            return "smtp.gmail.com";
        }

        public static string GetSmtpUser()
        {
            return "ysref.noreply@gmail.com";
        }

        public static string GetFromName()
        {
            return "ysref";
        }

        public static string GetFromEmail()
        {
            return "ysref.noreply@gmail.com";
        }

        public static string GetSmtpPassword()
        {
            return "ysref@2017";
        }

        public static int GetSmtpPort()
        {
            return 587;
        }

        public static bool GetSsl()
        {
            return true;
        }
    }
}
