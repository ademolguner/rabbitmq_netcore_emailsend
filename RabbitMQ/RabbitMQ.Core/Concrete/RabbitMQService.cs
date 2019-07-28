using RabbitMQ.Client;
using RabbitMQ.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Concrete
{
    public class RabbitMQService : IRabbitMQServices
    {
        public IRabbitMQConfiguration _RabbitMQConfiguration;

        public RabbitMQService()
        {
        }

        public RabbitMQService(IRabbitMQConfiguration RabbitMQConfiguration)
        {
            _RabbitMQConfiguration = RabbitMQConfiguration;
        }
        public IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _RabbitMQConfiguration.HostName,
                UserName = _RabbitMQConfiguration.UserName,
                Password = _RabbitMQConfiguration.Password
            };
            using (IConnection RabbitMQConnection = factory.CreateConnection())
            {
                return RabbitMQConnection;
            }
        }

        public IModel GetModel(IConnection connection)
        {
            using (IModel channel = connection.CreateModel())
            {
                return channel;
            }
        }
    }
}
