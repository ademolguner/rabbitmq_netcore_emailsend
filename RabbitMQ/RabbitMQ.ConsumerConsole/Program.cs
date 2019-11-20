using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Concrete;
using RabbitMQ.Core.Data;
using RabbitMQ.Core.Entities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Console;

namespace RabbitMQ.ConsumerConsole
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Program cs Acıldı.");
            var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("project.json");
            Console.WriteLine("project.json okundu.");
            var configuration = builder.Build();


            //setup our DI
            var serviceProvider = new ServiceCollection()
                //.AddSingleton<IConfiguration>()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IRabbitMQConfiguration, RabbitMQConfiguration>()
                .AddSingleton<IRabbitMQService, RabbitMQService>()
                .AddSingleton<IObjectConvertFormat, ObjectConvertFormatManager>()
                .AddSingleton<ISmtpConfiguration, SmtpConfiguration>()
                .AddSingleton<IMailSender, MailSender>()
                .AddSingleton<IDataModel<User>, UsersDataModel>()
                .AddSingleton<IConsumerService, IConsumerManager>()
                .BuildServiceProvider();
            Console.WriteLine("serviceProvider ve Dependency injectionlar okundu.");

            //services.AddSingleton<IConfiguration>(Configuration);






            //configure console logging
            //serviceProvider.GetService<ILoggerFactory>();

            //var logger = serviceProvider.GetService<ILoggerFactory>()
            //.CreateLogger<Program>();
            //logger.LogDebug("Starting application");


            var consumerService = serviceProvider.GetService<IConsumerService>();
            Console.WriteLine("consumerService alındı.");
            Console.WriteLine("consumerService.Start() başladı."+ DateTime.Now.ToShortTimeString());
            await consumerService.Start();
            Console.WriteLine("consumerService.Start() bitti." + DateTime.Now.ToShortTimeString());
            //logger.LogDebug("All done!");



        }

    }
}
