using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Abstract
{
    public interface IPublisherService
    {
        void Enqueue(IEnumerable<MailMessageData> messages);
    }
}
