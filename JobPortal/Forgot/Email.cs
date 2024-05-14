using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace JobPortal.Forgot
{
    public class Email
    {
        public class MailManager
        {
            private string MailSender { get; set; }
            private string MailAppPassword { get; set; }

            public MailManager()
            {
                MailSender = ConfigurationManager.AppSettings["MailSender"];
                MailAppPassword = ConfigurationManager.AppSettings["MailSenderAppPassword"];
            }

            public bool SendEmail(string szRecipient, string subject, string szMsgBody, ref string errResponse)
            {
                try
                {
                    using (var message = new MailMessage())
                    {
                        var smtp = new SmtpClient();
                        message.From = new MailAddress(MailSender);
                        message.To.Add(new MailAddress(szRecipient));
                        message.Subject = subject;
                        message.IsBodyHtml = true;
                        message.Body = szMsgBody;
                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(MailSender, MailAppPassword);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);

                        errResponse = "Message Sent";
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    errResponse = ex.Message;
                    return false;
                }
            }

        }
    }
}