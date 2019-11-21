using System.Collections.Generic;

namespace RabbitMQ.Core.Abstract
{
    public interface IPublisherService
    {
        void Enqueue<T>(IEnumerable<T> queueDataModels, string queueName ) where T: class, new();
    }
}

