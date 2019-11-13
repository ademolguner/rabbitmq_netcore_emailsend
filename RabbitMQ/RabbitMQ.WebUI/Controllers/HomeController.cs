using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Core.Entities;
using RabbitMQ.WebUI.Models;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Concrete;
using RabbitMQ.Consumer;
using RabbitMQ.Core.Consts;
using RabbitMQ.Core.Data;
using System.Linq;

namespace RabbitMQ.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRabbitMQServices _rabbitMQServices;
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        private readonly IObjectConvertFormat _objectConvertFormat;
        private Post post = new Post();
        private List<User> users = new List<User>();
        private readonly IDataModel<User> _userListData;
        public HomeController(IRabbitMQServices rabbitMQServices, IRabbitMQConfiguration rabbitMQConfiguration, IObjectConvertFormat objectConvertFormat, IDataModel<User> userListData)
        {
            _rabbitMQServices = rabbitMQServices;
            _rabbitMQConfiguration = rabbitMQConfiguration;
            _objectConvertFormat = objectConvertFormat;
            _userListData = userListData;
        }


        public IActionResult Index()
        {
            users = _userListData.GetData().ToList();
            post = new Post
            {
                Content = "RabbitMQ Deneme icerik",
                Title = "RabbitMQ Mail Gönderim İşlemi"
            };

            Publisher.Publisher publisher = new Publisher.Publisher(_rabbitMQServices);
            publisher.Enqueue(PrepareMessages());

            var receiver = new Receiver(_rabbitMQServices, _rabbitMQConfiguration, new MailSender(MailConsts.SmtpFileRoot, _objectConvertFormat), _objectConvertFormat);
            receiver.Start();
            
            return View();
        }



        private IEnumerable<MailMessageData> PrepareMessages()
        {
            var messages = new List<MailMessageData>();
            for (int i = 0; i < users.Count; i++)
            {
                messages.Add(new MailMessageData()
                {
                    To = users[i].Email.ToString(),
                    From = "ademolguner@gmail.com",
                    Subject = post.Title,
                    Body = post.Content
                });
            }
            return messages;
        }

        public IActionResult Privacy()
        {
            Publisher.Publisher publisher = new Publisher.Publisher(_rabbitMQServices);
            publisher.Enqueue(PrepareMessages());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
