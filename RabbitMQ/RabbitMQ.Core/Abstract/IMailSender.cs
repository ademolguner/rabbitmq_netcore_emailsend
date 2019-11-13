using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Core.Abstract
{
    public interface IMailSender
    {
        Task<MailSendResult> SendMailAsync(MailMessageData emailMessage);
    }
}
