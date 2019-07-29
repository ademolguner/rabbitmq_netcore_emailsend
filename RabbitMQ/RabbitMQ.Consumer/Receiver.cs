using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Consts;
using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
{
    public class Receiver : IDisposable
    {
        private readonly IRabbitMQServices _rabbitMQServices;
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        private readonly IObjectConvertFormat _objectConvertFormat;
        public Receiver(
            IRabbitMQServices rabbitMQServices,
            IRabbitMQConfiguration rabbitMQConfiguration,
            IObjectConvertFormat objectConvertFormat)
        {
            _rabbitMQServices = rabbitMQServices;
            _rabbitMQConfiguration = rabbitMQConfiguration;
            _objectConvertFormat = objectConvertFormat;
        }



        //public string QueueName { get; set; } = RabbitMqServiceConfiguration.GetConfiguration().QueueName;
        //public string Host { get; set; } = RabbitMqServiceConfiguration.GetConfiguration().Host;

        //Aynı anda - Eşzamanlı e-posta gönderimi sayısı birden fazla thread açma için sınır
        public ushort ParallelThreadsCount { get; set; } = 3;
        // Multithread yöneticisi
        private SemaphoreSlim _semaphore;

        // eventler - olaylar
        public event EventHandler<MailMessageData> MessageReceived;
        public event EventHandler<MailSendResult> MessageProcessed;
        private readonly IMailSender _mailSender;
        private EventingBasicConsumer _consumer;
        private IModel _channel;
        private IConnection _connection;


        public Receiver(IMailSender mailSender)
        {
            _mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
        }

        // Mesaj almaya başlanır
        public void Start()
        {
            _semaphore = new SemaphoreSlim(ParallelThreadsCount);

            var factory = new ConnectionFactory() { HostName = _rabbitMQConfiguration.HostName };
            using (var _connection = _rabbitMQServices.GetConnection())
            using (var _channel = _connection.CreateModel())
            {


                _channel.QueueDeclare(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),     //
                                     durable: true,     //
                                     exclusive: false,  //
                                     autoDelete: false, //
                                     arguments: null);  //

                //Onaylanmayan maksimum mesaj sayısını sınırlayın.
                //Aracı, alınana kadar yeni bir mesaj vermez.
                //daha önce kabul edilenlerden birinin alındığına dair onay
                _channel.BasicQos(0, ParallelThreadsCount, false);

                _consumer = new EventingBasicConsumer(_channel);
                _consumer.Received += Consumer_Received;
                _channel.BasicConsume(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                         autoAck: false,         /* bir mesajı aldıktan sonra bunu anladığına 
                                                                dair(acknowledgment) kuyruğa bildirimde bulunur ya da timeout gibi vakalar oluştuğunda 
                                                                mesajı geri çevirmek(Discard) veya yeniden kuyruğa aldırmak(Re-Queue) için dönüşler yapar*/
                                         consumer: _consumer);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                _semaphore.Wait();

                var message = _objectConvertFormat.ParseObjectDataArray<MailMessageData>(ea.Body);
                MessageReceived?.Invoke(this, message);
                // E-Posta akışını başlatma yeri
                Task.Run(() =>
                {
                    try
                    {
                        var task = _mailSender.SendMailAsync(message);
                        task.Wait();
                        var result = task.Result;
                        MessageProcessed?.Invoke(this, result);
                    }
                    finally
                    {
                    // Teslimat Onayı
                    _channel.BasicAck(ea.DeliveryTag, false);
                    // akışı thread i serbest bırakıyoruz ek thread alabiliriz
                    _semaphore.Release();
                    }
                });
            }
            catch (Exception ex)
            {
                string hata = ex.InnerException.ToString();
            }
        }



        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
