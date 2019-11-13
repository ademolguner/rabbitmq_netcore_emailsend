using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Data
{
    public interface IDataModel<T>
    {
        IEnumerable<T> GetData();
    }
}
