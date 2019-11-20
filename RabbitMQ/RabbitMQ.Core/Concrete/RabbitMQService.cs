using RabbitMQ.Client;
using RabbitMQ.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Concrete
{
    public class RabbitMQService : IRabbitMQService
    {
        public IRabbitMQConfiguration _rabbitMQConfiguration;
        public RabbitMQService(IRabbitMQConfiguration RabbitMQConfiguration)
        {
            _rabbitMQConfiguration = RabbitMQConfiguration;
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
