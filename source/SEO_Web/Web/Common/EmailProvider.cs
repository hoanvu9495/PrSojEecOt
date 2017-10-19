using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using Model.DBTool;
using Business.Business;
using Web.Common;
namespace Web.Common
{
    public class EmailProvider
    {
        private DmNguoidungBusiness dmNguoiDungBusiness;
        private static string ACCOUNT_MAIL = WebConfigurationManager.AppSettings["ACCOUNT_MAIL"];
        private static string PASS_MAIL = WebConfigurationManager.AppSettings["PASS_MAIL"];
        private static string HOST_WEB = WebConfigurationManager.AppSettings["HOST_WEB"];
        public static bool sendEmail(string body, string subject, List<string> address)
        {
            SmtpClient server = new SmtpClient();

            try
            {
                string from = ACCOUNT_MAIL;

                server.Host = "smtp.gmail.com";
                server.Port = 587;

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new MailAddress(from);
                foreach (var item in address)
                {
                    mail.To.Add(item);
                }

                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                server.Credentials = new NetworkCredential(from, PASS_MAIL);
                server.EnableSsl = true;
                server.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}