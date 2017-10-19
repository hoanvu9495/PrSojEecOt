using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Web.Common
{
    public class SmtpMailer
    {
        public static void sendMail(string to, string from, string subject, string body)
        {
            /// Using default Smtp config (config in Web.config)
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            /// Mail details
            MailMessage msg = new MailMessage(from, to);

            try
            {
                msg.From = new MailAddress(from, "Aita-Quản lý điều hành");
                msg.To.Add(to);
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.Body = body;
                msg.Priority = MailPriority.Normal;

                /// Enable one of the following method.
                client.Port = 587;
                client.Credentials = new NetworkCredential(from, "HNegovofficea@");
                client.Send(msg);
            }
            catch (Exception exp)
            {
                /// Throw exception to higher tier
                throw exp;
            }
        }

        public static void sendMail(IList<string> to, string from, string subject, string body)
        {
            foreach (var t in to)
                sendMail(t, from, subject, body);
        }
    }
}