using System;
using System.ComponentModel;
using System.IO;
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
        private IConnection connection;
        private Credentials loginCredentials;
        private DateTime lastPong;
        private DateTime lastLoginAttempt;
        private Timer keepAliveTimer;

        /// <summary>
        /// Gets or sets the message parser.
        /// </summary>
        /// <value>
        /// The message parser.
        /// </value>
        public IMessageParser MessageParser { get; set; }

        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        public IConsole Console { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        public ChatService()
        {
            connection = new Connection();

            var worker = new BackgroundWorker();

            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();

            keepAliveTimer = new Timer(KeepAlive, null, 60000, 60000);
        }

        private void KeepAlive(object state)
        {
            if (!LoggedIn) return;

            var lastPongSeconds = (DateTime.Now - lastPong).TotalSeconds;

            if (lastPongSeconds > 180)
            {
                Console.WriteLine("Lost connection to server, logging in again...");

                LoggedIn = false;

                connection.Dispose();
                connection = new Connection();

                AttemptLogin(false);
            }
            else
            {
                Console.WriteLine("Last PONG: {0:0} secs.", lastPongSeconds);

                connection.GetSocketStatus();
            }
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
                if (!LoggedIn)
                {
                    Thread.Sleep(1000);

                    AttemptLogin(true);

                    continue;
                }

                string data;

                try
                {
                    data = connection.Receive();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType() + ": " + ex.Message);

                    connection.Dispose();
                    connection = new Connection();

                    LoggedIn = false;

                    continue;
                }

                // Check received data
                if (string.IsNullOrWhiteSpace(data)) continue;

                Console.WriteLine(">> " + data);

                var message = MessageParser.Parse(data);

                // Handle IRC Ping
                if (message.Type == MessageType.Ping)
                {
                    Send("PONG {0}", message.Body);

                    lastPong = DateTime.Now;

                    continue;
                }

                // Handle Set Mode Message
                if (message.Type == MessageType.SetMode)
                {
                    // Raise event
                    OnLogin(this, new LoginEventArgs());

                    continue;
                }

                // Fire event
                OnMessage(this, new MessageEventArgs { Message = message });
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
            loginCredentials = credentials;

            if (connection.Connect(credentials.Server, credentials.Port))
            {
                connection.Send("PASS {0}", credentials.Password);
                connection.Send("USER {0} {0} {0} :{1}", credentials.Nick, credentials.Name);
                connection.Send("NICK {0}", credentials.Nick);

                LoggedIn = true;

                lastPong = DateTime.Now;

                // Assign Context information
                Context.Nick = credentials.Nick;
            }
        }

        /// <summary>
        /// Attempts to login to the server.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void AttemptLogin(bool checkTime)
        {
            // Only Attempt to login every 60 seconds
            if (checkTime)
            {
                if ((DateTime.Now - lastLoginAttempt).TotalSeconds < 60)
                {
                    return;
                }
            }

            // Check login credentials have been set
            if (loginCredentials == null)
            {
                return;
            }

            lastLoginAttempt = DateTime.Now;

            Console.WriteLine("Attempting to connect to: {0}", loginCredentials.Server);

            Login(loginCredentials);
        }

        /// <summary>
        /// Joins the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns></returns>
        public bool Join(string channel)
        {
            Send("JOIN #{0}", channel);

            return true;
        }

        /// <summary>
        /// Leaves the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public bool Leave(string channel)
        {
            Send("PART #{0}", channel);

            return true;
        }

        public void Reply(string channel, string response)
        {
            // Send message
            Send("PRIVMSG {0} :{1}", channel, response);
        }

        /// <summary>
        /// Replies to the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="response">The response.</param>
        /// <param name="args">The args.</param>
        public void ReplyFormat(string channel, string response, params object[] args)
        {
            Reply(channel, string.Format(response, args));
        }

        /// <summary>
        /// Says the message in the specified channel.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(string message)
        {
            connection.Send(message);
        }

        /// <summary>
        /// Says the message in the specified channel.
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
            Reply(to, response);
        }

        /// <summary>
        /// Replies the specified message with the given response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The response.</param>
        /// <param name="args">The args.</param>
        public void ReplyFormat(Message message, string response, params object[] args)
        {
            Reply(message, string.Format(response, args));
        }
    }
}
