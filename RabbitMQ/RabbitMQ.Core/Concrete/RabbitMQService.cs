using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMQ.Core.Concrete
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        public RabbitMQService(IRabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration ?? throw new ArgumentNullException(nameof(rabbitMQConfiguration));
        }
        public  IConnection GetConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQConfiguration.HostName,
                    UserName = _rabbitMQConfiguration.UserName,
                    Password = _rabbitMQConfiguration.Password
                };

                // Otomatik bağlantı kurtarmayı etkinleştirmek için,
                factory.AutomaticRecoveryEnabled = true;
                // Her 10 sn de bir tekrar bağlantı toparlanmaya çalışır 
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                // sunucudan bağlantısı kesildikten sonra kuyruktaki mesaj tüketimini sürdürmez 
                // (TopologyRecoveryEnabled = false   olarak tanımlandığı için)
                factory.TopologyRecoveryEnabled = false;

                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
               // loglama işlemi yapabiliriz
                Thread.Sleep(5000);
                // farklı business ta yapılabilir, ancak biz tekrar bağlantı (connection) kurmayı deneyeceğiz
                return GetConnection();
            }
        }

        public IModel GetModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
