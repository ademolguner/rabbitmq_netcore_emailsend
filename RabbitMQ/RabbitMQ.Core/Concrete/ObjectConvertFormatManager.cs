using Newtonsoft.Json;
using RabbitMQ.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Concrete
{
    public class ObjectConvertFormatManager : IObjectConvertFormat
    {
        public T JsonToObject<T>(string jsonString)
        {
            var message = JsonConvert.DeserializeObject<T>(jsonString);
            return message;
        }

        public string ObjectToJson<T>(T message)
        {
            var jsonString = JsonConvert.SerializeObject(message);
            return jsonString;
        }

        
    }
}
