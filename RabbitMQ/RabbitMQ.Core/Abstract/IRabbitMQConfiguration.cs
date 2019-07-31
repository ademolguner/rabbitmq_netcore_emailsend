using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Abstract
{
    public interface IRabbitMQConfiguration
    { 
        string HostName { get; }
        string UserName { get; }
        string Password { get; }
         
    }
}
