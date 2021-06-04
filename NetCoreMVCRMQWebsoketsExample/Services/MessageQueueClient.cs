using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using NetCoreMvcDIExample.Helpers;
using NetCoreMVCRMQWebsoketsExample.Hubs;
using NetCoreMVCRMQWebsoketsExample.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreMVCRMQWebsoketsExample.Services
{
    /// <summary>
    /// Connector implementation
    /// </summary>
    public class MessageQueueClient:IMessageQueueClient
    {
        /// <summary>
        /// Connection to RMQ Server
        /// </summary>
        private IConnection _connection;

        /// <summary>
        /// Channel rabbitmq
        /// </summary>
        private IModel _model;

        /// <summary>
        /// Consumer
        /// </summary>
        private EventingBasicConsumer _consummer;

        /// <summary>
        /// Connection settings
        /// </summary>
        private RMQConnectionSettings _settings;

        /// <summary>
        /// Hub context to send message to client browser
        /// </summary>
        private IHubContext<MessageHub> _hubContext;

        /// <summary>
        /// ctor with injecting configuration and hubContext
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hubContext"></param>
        public MessageQueueClient(IConfiguration configuration, IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
            _settings = LoadSettings(configuration);

            //Creating factory with parameters
            ConnectionFactory factorty = new ConnectionFactory()
            {
                UserName = _settings.User,
                Password = _settings.Password,
                VirtualHost = _settings.Vhost,
                HostName = _settings.Host,
                Port = _settings.Port
            };

            try
            {
                //Initialize connection
                _connection = factorty.CreateConnection();
                //initialize channel
                _model = _connection.CreateModel();
                //bind to queue
                _model.QueueDeclare(_settings.Queue,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
                //initialize consuming
                Subscribe(_settings.Queue);
            }
            catch (Exception ex)
            {
               //TODO: Log exception
            }
        }

        /// <summary>
        /// Load settings from appsettings
        /// add to appsettings
        /// "RMQ": {
        ///    "host": "127.0.0.1",
        ///    "user": "guest",
        ///    "password": "guest",
        ///    "vhost": "/",
        ///    "port": 5672,
        ///    "queue":  "TestQueue1"
        ///}
        /// </summary>        
        /// <returns></returns>
        private RMQConnectionSettings LoadSettings(IConfiguration configuration)
        {
            var settings = new RMQConnectionSettings();
            settings.Host = configuration["RMQ:host"];
            settings.User = configuration["RMQ:user"];
            settings.Password = configuration["RMQ:password"];
            settings.Vhost = configuration["RMQ:vhost"];
            
            int port = 0;  
            settings.Port = int.TryParse(configuration["RMQ:port"], out port) ? port : 5672 ;

            settings.Queue = configuration["RMQ:queue"]; 
            return settings;
        }

        /// <summary>
        /// SendMessage. Send data to queue
        /// Actualy this solution not right. You must to implement only simple send byte[]/headers e.t.c
        /// and converting object to byte[] at another helper             
        /// But for example - why not?
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(MessageExample message)
        {
            if (_model != null && _model.IsOpen && message!= null)
            {
                var messageString = XMLHelper.Serialize(message);
                if (messageString != null)
                {
                    var data = Encoding.UTF8.GetBytes(messageString);
                    var prop = _model.CreateBasicProperties();
                    //NB! sending always to exchange. After install rmq create default exhange with empty name.
                    //For example - send to default exhange with routingkey=queue name.
                    _model.BasicPublish(_settings.Exchange == null ? "" : _settings.Exchange, _settings.Queue, prop, data);
                }
            }
        }

        /// <summary>
        /// Subscribing to queue
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public string Subscribe(string queue)
        {
            if (_model != null && _model.IsOpen)
            {               
                _model.BasicQos(0, 20, false);
                _consummer = new EventingBasicConsumer(_model);               
                _consummer.Received += OnReceaved;                                     
                _consummer.ConsumerCancelled += (ch, ea) =>
                {
                    //TODO: Log about cancell consume
                };               
                _consummer.Shutdown += (ch, ea) =>
                {
                   //TODO: Log about shutdown
                };
                _model.BasicConsume(queue, true, _consummer);
            }
            return "";
        }

        /// <summary>
        /// On receave event processor. It read data and send it to all MessageHub clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceaved(object sender, BasicDeliverEventArgs e)
        {
            string dataString = Encoding.UTF8.GetString(e.Body.ToArray());
            if (dataString != null)
            {
                var message = XMLHelper.Deserialize<MessageExample>(dataString);
                if (message != null)
                {
                    _hubContext.Clients.All.SendAsync("NewMessage", message.MessageText, message.CreationDate.ToString());
                }
            }
        }
    }
}
