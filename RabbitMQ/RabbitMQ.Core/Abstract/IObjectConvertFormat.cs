using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Abstract
{
   public interface IObjectConvertFormat
    {
        T JsonToObject<T>(string jsonString);
        string ObjectToJson<T>(T message);
    }
}
