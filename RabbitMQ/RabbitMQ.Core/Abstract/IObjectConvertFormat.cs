using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Abstract
{
   public interface IObjectConvertFormat
    {
        T JsonToObject<T>(string jsonString) where T: class, new();
        string ObjectToJson<T>(T entityObject) where T : class, new();
        T ParseObjectDataArray<T>(byte[] rawBytes) where T : class, new();
    }
}
