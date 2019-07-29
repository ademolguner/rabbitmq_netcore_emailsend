using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RabbitMQ.Core.Consts
{
  public  class RabbitMQConsts
    {
      public   enum RabbitMqConstsList
        {
            [Description("QueueNameEmail")]
            QueueNameEmail =1,
            [Description("QueueNameSms")]
            QueueNameSms = 2
        }

    }
}
