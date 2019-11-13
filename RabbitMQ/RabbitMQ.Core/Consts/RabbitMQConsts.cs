using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RabbitMQ.Core.Consts
{
    public class RabbitMQConsts
    {

        /// yaşam süresi
        public static int MessagesTTL { get; set; } = 1000 * 60 * 60 * 2;

        //Aynı anda - Eşzamanlı e-posta gönderimi sayısı birden fazla thread açma için sınır
        public static ushort ParallelThreadsCount { get; set; } = 3;
        public enum RabbitMqConstsList
        {
            [Description("QueueNameEmail")]
            QueueNameEmail = 1,
            [Description("QueueNameSms")]
            QueueNameSms = 2
        }

    }
}
