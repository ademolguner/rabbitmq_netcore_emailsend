using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace RabbitMQ.Core.Entities
{
    public class MailMessageData
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public MailMessage GetMailMessage()
        {
            var mailMessage = new MailMessage
            {
                Subject = this.Subject,
                Body = this.Body,
                From = new MailAddress(this.From)
            };
            mailMessage.To.Add(To);
            return mailMessage;
        }


    }
}
