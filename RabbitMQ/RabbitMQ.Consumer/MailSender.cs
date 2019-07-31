using Newtonsoft.Json;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
{
    public class MailSender : IMailSender
    {


        public string SmtpConfigPath { get; set; }
        public int SendTimeout { get; set; } = 250;
        public MailSender(string smtpConfigPath)
        {
            SmtpConfigPath = smtpConfigPath;
        }
        public SmtpConfig GetConfig()
        {
            return JsonConvert.DeserializeObject<SmtpConfig>(File.ReadAllText(SmtpConfigPath));
        }


        public async Task<MailSendResult> SendMailAsync(MailMessageData emailMessage)
        {
            MailSendResult result;
            MailMessage mailMessage = emailMessage.GetMailMessage();
            try
            {
                using (var client = CreateSmtpClient(GetConfig()))
                {
                    await client.SendMailAsync(mailMessage);
                    string resultMessage = $"totooo {string.Join(",", mailMessage.To)}.";
                    result = new MailSendResult(mailMessage, true, resultMessage);
                    Console.WriteLine("EmailRabbitMQProcessor running => resultMessage to: " + mailMessage.To);
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
                Thread.Sleep(SendTimeout);
            }
            return result;
        }


        private SmtpClient CreateSmtpClient(SmtpConfig config)
        {
            SmtpClient client = new SmtpClient(config.Host, config.Port);
            client.EnableSsl = config.UseSSL;

            client.UseDefaultCredentials = !(string.IsNullOrWhiteSpace(config.User) && string.IsNullOrWhiteSpace(config.Password));
            if (client.UseDefaultCredentials == true)
            {
                client.Credentials = new NetworkCredential(config.User, config.Password);
            }

            return client;
        }

       
    }
}
