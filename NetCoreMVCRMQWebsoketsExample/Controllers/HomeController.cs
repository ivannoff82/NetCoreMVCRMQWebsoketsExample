using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreMVCRMQWebsoketsExample.Models;
using NetCoreMVCRMQWebsoketsExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample.Controllers
{
    public class HomeController : Controller
    {      
        /// <summary>
        /// Injected queue service
        /// </summary>
        private IMessageQueueClient _messageQueueClient;
        
        /// <summary>
        /// ctor for inject rabbitmq client service
        /// </summary>
        /// <param name="messageQueueClient"></param>
        public HomeController (IMessageQueueClient messageQueueClient) {
           
            _messageQueueClient = messageQueueClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string SendMessage(string message)
        {
            if (_messageQueueClient != null)
            {
                _messageQueueClient.SendMessage(new MessageExample(message));
            }
            return "0";
        }
    }    
}
