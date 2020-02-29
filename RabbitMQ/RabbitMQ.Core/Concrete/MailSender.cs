using Newtonsoft.Json;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Consts;
using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Core.Concrete
{
    public class MailSender : IMailSender
    {
        
        private readonly ISmtpConfiguration _smtpConfiguration;
        public MailSender(ISmtpConfiguration smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration;
        }

        public async Task<MailSendResult> SendMailAsync(MailMessageData emailMessage)
        {
            Console.WriteLine($"EmailRabbitMQProcessor SendMailAsync method  => Calısma zamanı: {DateTime.Now.ToShortTimeString()}");

            MailSendResult result;
            MailMessage mailMessage = emailMessage.GetMailMessage();
            mailMessage.From = new MailAddress(_smtpConfiguration.User);
            try
            {
                using (var client = CreateSmtpClient(_smtpConfiguration.GetSmtpConfig()))
                {
                    await client.SendMailAsync(mailMessage);
                    string resultMessage = $"donus mesajı metni  {string.Join(",", mailMessage.To)}.";
                    result = new MailSendResult(mailMessage, true, resultMessage);
                    Console.WriteLine($"EmailRabbitMQProcessor running => resultMessage to:{ mailMessage.To}");
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                result = new MailSendResult(mailMessage, false, $"Hata: {ex.Message}");
            }
            finally
            {
                Thread.Sleep(MailConsts.SendTimeout);
            }
            return result;
        }

        private SmtpClient CreateSmtpClient(SmtpConfig config)
        {
            SmtpClient client = new SmtpClient(config.Host, config.Port)
            {
                EnableSsl = config.UseSsl,
                UseDefaultCredentials = !(string.IsNullOrWhiteSpace(config.User) && string.IsNullOrWhiteSpace(config.Password))
            };
            if (client.UseDefaultCredentials == true)
            {
                client.Credentials = new NetworkCredential(config.User, config.Password);
            }
            return client;
        }



       
    }
}
