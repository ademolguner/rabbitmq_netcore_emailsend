using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RabbitMQ.Core.Consts
{
    public static class RabbitMQConsts
    {

        /// yaşam süresi
        public static int MessagesTTL { get; set; } = 1000 * 60 * 60 * 2;

        //Aynı anda - Eşzamanlı e-posta gönderimi sayısı, thread açma için sınırı belirleriz
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
