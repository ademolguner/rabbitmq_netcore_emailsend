using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Core.Entities;
using RabbitMQ.WebUI.Models;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Concrete;

namespace RabbitMQ.WebUI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IRabbitMQServices _rabbitMQServices;
        private readonly IRabbitMQConfiguration _rabbitMQConfiguration;
        

        public HomeController(IRabbitMQServices rabbitMQServices, IRabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQServices = rabbitMQServices;
            _rabbitMQConfiguration = rabbitMQConfiguration; 
        }

        Post post = new Post();
        List<User> users = new List<User>();
        public IActionResult Index()
        {

            User _user = new User();
            users = _user.GetUserList();
            post = new Post()
            {
                Content = "Deneme",
                Title = "RabbitMQ "
            };

            Publisher.Publisher publisher = new Publisher.Publisher(_rabbitMQServices, _rabbitMQConfiguration);
            publisher.Enqueue(PrepareMessages());

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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
