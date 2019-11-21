using RabbitMQ.Client;
using RabbitMQ.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Concrete
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        public RabbitMQService(IRabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration ?? throw new ArgumentNullException(nameof(rabbitMQConfiguration));
        }
        public IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfiguration.HostName,
                UserName = _rabbitMQConfiguration.UserName,
                Password = _rabbitMQConfiguration.Password
            }; 
            return factory.CreateConnection();
        }

        public IModel GetModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
