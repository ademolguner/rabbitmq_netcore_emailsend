using RabbitMQ.Core.Abstract;
using System.Text;
using Newtonsoft.Json;

namespace RabbitMQ.Core.Concrete
{
    public class ObjectConvertFormatManager : IObjectConvertFormat
    {
        public T JsonToObject<T>(string jsonString) where T : class, new()
        {
            var objectData = JsonConvert.DeserializeObject<T>(jsonString);
            return objectData;
        }

        public string ObjectToJson<T>(T entityObject) where T : class, new()
        {
            var jsonString = JsonConvert.SerializeObject(entityObject);
            return jsonString;
        }

        public T ParseObjectDataArray<T>(byte[] rawBytes) where T : class, new()
        {
            var stringData = Encoding.UTF8.GetString(rawBytes);
            return JsonToObject<T>(stringData);
        }

    }
}

