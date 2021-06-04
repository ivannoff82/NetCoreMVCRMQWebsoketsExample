using NetCoreMVCRMQWebsoketsExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample.Services
{
    /// <summary>
    /// Simple processor for connect to rabbit
    /// </summary>
    public interface IMessageQueueClient
    {
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message"></param>
        void SendMessage(MessageExample message);
    }
}
