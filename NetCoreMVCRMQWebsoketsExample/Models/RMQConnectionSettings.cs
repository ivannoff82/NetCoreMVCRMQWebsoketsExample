using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample.Models
{
    /// <summary>
    /// Rabbitmq simple connection setings model
    /// </summary>
    public class RMQConnectionSettings
    {      
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string  Vhost { get; set; }
        public int Port { get; set; }
        public string Queue { get; set; }
        public string Exchange { get; set; }
    }
}
