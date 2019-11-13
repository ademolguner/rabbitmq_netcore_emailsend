using Newtonsoft.Json;
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
        // Multithread yöneticisi
        private SemaphoreSlim _semaphore;
        // eventler - olaylar
        public event EventHandler<MailMessageData> MessageReceived;
        public event EventHandler<MailSendResult> MessageProcessed;

        private EventingBasicConsumer _consumer;
        private IModel _channel;
        private IConnection _connection;

        private readonly IRabbitMQServices _rabbitMQServices;
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        private readonly IObjectConvertFormat _objectConvertFormat;
        private readonly IMailSender _mailSender;
        public Receiver(
            IRabbitMQServices rabbitMQServices,
            IRabbitMQConfiguration rabbitMQConfiguration,
            IMailSender mailSender,
            IObjectConvertFormat objectConvertFormat
            )
        {
            _rabbitMQServices = rabbitMQServices;
            _rabbitMQConfiguration = rabbitMQConfiguration;
            _mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
            _objectConvertFormat = objectConvertFormat;
        }





        // Mesaj almaya başlanır
        public void Start()
        {
            try
            {
                _semaphore = new SemaphoreSlim(RabbitMQConsts.ParallelThreadsCount);

                var factory = new ConnectionFactory() { HostName = _rabbitMQConfiguration.HostName };
                _connection = _rabbitMQServices.GetConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                     durable: true,     
                                     exclusive: false,  
                                     autoDelete: false, 
                                     arguments: null);  

                _channel.BasicQos(0, RabbitMQConsts.ParallelThreadsCount, false);
                _consumer = new EventingBasicConsumer(_channel);
                _consumer.Received += Consumer_Received;
                _channel.BasicConsume(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                         autoAck: false,         /* bir mesajı aldıktan sonra bunu anladığına 
                                                                dair(acknowledgment) kuyruğa bildirimde bulunur ya da timeout gibi vakalar oluştuğunda 
                                                                mesajı geri çevirmek(Discard) veya yeniden kuyruğa aldırmak(Re-Queue) için dönüşler yapar*/
                                         consumer: _consumer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                _semaphore.Wait();
                MailMessageData message = _objectConvertFormat.JsonToObject<MailMessageData>(Encoding.UTF8.GetString(ea.Body));
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
                    catch (Exception ex)
                    {
                        throw new Exception(ex.InnerException.Message.ToString());
                    }
                    finally
                    {
                        // Teslimat Onayı
                        _channel.BasicAck(ea.DeliveryTag, false);
                        // akışı - thread'i serbest bırakıyoruz ek thread alabiliriz.
                        _semaphore.Release();
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.ToString());
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
            _semaphore.Dispose();
        }
    }
}
