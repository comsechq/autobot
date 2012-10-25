using System;
using System.ComponentModel;
using System.Threading;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Events;

namespace AutoBot.Services
{
    /// <summary>
    /// Service for interacting with the IRC network
    /// </summary>
    public class ChatService : IChatService
    {
        private readonly IConnection connection;

        /// <summary>
        /// Gets or sets the message parser.
        /// </summary>
        /// <value>
        /// The message parser.
        /// </value>
        public IMessageParser MessageParser { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        public ChatService()
        {
            connection = new Connection();

            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }
      
        #region Events
        
        /// <summary>
        /// Occurs when the Bot logs in
        /// </summary>
        public event EventHandler<LoginEventArgs> OnLogin;

        /// <summary>
        /// Occurs when the Bot receives a message
        /// </summary>
        public event EventHandler<MessageEventArgs> OnMessage;

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the processing of incoming data from the IRC server
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                // Check logged in
                if (!LoggedIn) continue;

                var data = connection.Receive();

                // Check received data
                if (string.IsNullOrWhiteSpace(data)) continue;

                Console.WriteLine(">> " + data);

                var message = MessageParser.Parse(data);

                // Handle IRC Ping
                if (message.Type == MessageType.Ping)
                {
                    Send("PONG {0}", message.Body);
                    continue;
                }

                // Fire event
                OnMessage(this, new MessageEventArgs { Message = message });

                // Don't consume CPU
                Thread.Sleep(1000);
            }
        }
  
        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logged in; otherwise, <c>false</c>.
        /// </value>
        public bool LoggedIn { get; private set; }

        /// <summary>
        /// Logs in with the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public void Login(Credentials credentials)
        {
            if (connection.Connect(credentials.Server, credentials.Port))
            {
                connection.Send("PASS {0}", credentials.Password);
                connection.Send("USER {0} {0} {0} :{1}", credentials.Nick, credentials.Name);
                connection.Send("NICK {0}", credentials.Nick);

                LoggedIn = true;

                // Assign Context information
                Context.Nick = credentials.Nick;

                // Raise event
                OnLogin(this, new LoginEventArgs());
            }
        }

        /// <summary>
        /// Joins the specified channel.
        /// </summary>
        /// <param name="channel">The room.</param>
        /// <returns></returns>
        public bool Join(string channel)
        {
            Send("JOIN #{0}", channel);

            return true;
        }

        /// <summary>
        /// Leaves the specified channel.
        /// </summary>
        /// <param name="channel">The room.</param>
        public bool Leave(string channel)
        {
            Send("PART #{0}", channel);

            return true;
        }

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(string message)
        {
            connection.Send(message);
        }

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public void Send(string message, params object[] args)
        {
            Send(string.Format(message, args));
        }

        /// <summary>
        /// Replies the specified message with the given respons.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The response.</param>
        public void Reply(Message message, string response)
        {
            var to = message.From;

            // Check for a group reply
            if (message.To.StartsWith("#"))
            {
                to = message.To;
            }

            // Send message
            Send("PRIVMSG {0} :{1}", to, response);
        }

        /// <summary>
        /// Replies the specified message with the given response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The response.</param>
        /// <param name="args">The args.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Reply(Message message, string response, params object[] args)
        {
            Reply(message, string.Format(response, args));
        }

        /// <summary>
        /// Sets the bot's status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public void SetStatus(Status status, string message)
        {
            // Ensure we're logged in
            //if (!LoggedIn) return;         

            //switch (status)
            //{
            //    case Status.Available:
            //        connection.Show = ShowType.NONE;
            //        break;

            //    case Status.Away:
            //        connection.Show = ShowType.away;
            //        break;
                    
            //    case Status.Busy:
            //        connection.Show = ShowType.dnd;
            //        break;

            //    default:
            //        throw new ApplicationException("Unknown status type: " + status);

            //}

            //connection.Status = message;

            //connection.SendMyPresence();
        }
    }
}
