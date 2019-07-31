using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Concrete;
using RabbitMQ.Core.Consts;
using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Publisher
{
    public class Publisher
    {
        private readonly IRabbitMQServices _rabbitMQServices;
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        private readonly ObjectConvertFormatManager _objectConvertFormatManager;



        public Publisher(IRabbitMQServices rabbitMQServices, IRabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQServices = rabbitMQServices;
            _rabbitMQConfiguration = rabbitMQConfiguration;
        }

        // yaşam süresi
        public int MessagesTTL { get; set; } = 1000 * 60 * 60 * 2;


        // publisher işlemi
        public void Enqueue(IEnumerable<MailMessageData> messages)
        {
            try
            {
                
                using (var connection = _rabbitMQServices.GetConnection())
                using (var channel = connection.CreateModel())
                {
                     
                    channel.QueueDeclare(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                         durable: true,      // ile in-memory mi yoksa fiziksel olarak mı saklanacağı belirlenir.
                                         exclusive: false,   // yalnızca bir bağlantı tarafından kullanılır ve bu bağlantı kapandığında sıra silinir - özel olarak işaretlenirse silinmez
                                         autoDelete: false,  // en son bir abonelik iptal edildiğinde en az bir müşteriye sahip olan kuyruk silinir
                                         arguments: null);   // isteğe bağlı; eklentiler tarafından kullanılır ve TTL mesajı, kuyruk uzunluğu sınırı, vb. 

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;  // kalıcı
                    properties.Expiration = MessagesTTL.ToString();

                    foreach (var message in messages)
                    {
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));// _objectConvertFormatManager.ObjectToJson<MailMessageData>(message));
                        channel.BasicPublish(exchange: "",
                                             routingKey: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                             basicProperties: properties,
                                             body: body);
                    }
                }
            }
            catch (Exception ex)
            {
                //string hata = ex.InnerException.ToString();
            }
        }
    }
}
