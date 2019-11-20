using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Abstract
{
    public interface ISmtpConfiguration
    {
        string Host { get; }
        int Port { get; }
        string User { get; }
        string Password { get; }
        bool UseSSL { get; }
        SmtpConfig GetSmtpConfig();
    }
}
