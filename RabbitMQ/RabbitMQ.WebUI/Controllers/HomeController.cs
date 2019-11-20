using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Core.Entities;
using RabbitMQ.WebUI.Models;
using RabbitMQ.Core.Abstract;
using RabbitMQ.Core.Data;
using System.Linq;
using RabbitMQ.WebUI.ViewModel;

namespace RabbitMQ.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISmtpConfiguration _smtpConfig;
        private readonly IPublisherService _publisherService;
        private readonly IDataModel<User> _userListData;
        public HomeController(IDataModel<User> userListData, ISmtpConfiguration smtpConfig, IPublisherService publisherService)
        {
            _userListData = userListData;
            _smtpConfig = smtpConfig;
            _publisherService = publisherService;
        }


        public IActionResult Index()
        {
            return View();
            
        }


        [HttpGet]
        public IActionResult MailSend()
        { 
            return View();
        }


        [HttpPost]
        public IActionResult MailSend(PostMailViewModel postMailViewModel)
        {
            _publisherService.Enqueue(PrepareMessages(postMailViewModel));
             
            return View();
        }

        [HttpPost]
        public IActionResult MailSend2(PostMailViewModel postMailViewModel)
        {
            return View(postMailViewModel);
        }
        private IEnumerable<MailMessageData> PrepareMessages(PostMailViewModel postMailViewModel)
        {
            var users = _userListData.GetData().ToList();
            var messages = new List<MailMessageData>();
            for (int i = 0; i < users.Count; i++)
            {
                messages.Add(new MailMessageData()
                {
                    To = users[i].Email.ToString(),
                    From = _smtpConfig.User,
                    Subject = postMailViewModel.Post.Title,
                    Body = postMailViewModel.Post.Content
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
