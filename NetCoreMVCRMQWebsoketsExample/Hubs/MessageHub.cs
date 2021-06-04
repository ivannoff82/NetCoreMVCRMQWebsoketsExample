using Microsoft.AspNetCore.SignalR;
using NetCoreMVCRMQWebsoketsExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample.Hubs
{
    /// <summary>
    /// Messagr hub - only for demonstration RMQ send/consume
    /// </summary>
    public class MessageHub : Hub
    {
        /// <summary>
        /// Send message for all clients
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMessageToClient(MessageExample message)
        {
            return Clients.All.SendAsync("NewMessage", message.MessageText, message.CreationDate.ToString());
        }
    }
}
