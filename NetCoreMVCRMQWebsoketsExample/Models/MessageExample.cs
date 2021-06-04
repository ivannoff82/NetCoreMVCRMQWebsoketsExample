using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample.Models
{
    /// <summary>
    /// Message with date example model
    /// </summary>
    public class MessageExample
    {
        /// <summary>
        /// date of creation message
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Message body
        /// </summary>
        public string MessageText { get; set; }

        public MessageExample()
        {
            CreationDate = DateTime.Now;
        }

        public MessageExample( string message)
        {
            CreationDate = DateTime.Now;
            MessageText = message;
        }
    }
}
