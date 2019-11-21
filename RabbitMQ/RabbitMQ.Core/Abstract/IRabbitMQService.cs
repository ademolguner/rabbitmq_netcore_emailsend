using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Abstract
{
    public interface IRabbitMQService
    {
        IModel GetModel();
    }
}
