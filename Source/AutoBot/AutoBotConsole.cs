using System;
using System.Threading;
using AutoBot.Domain;
using AutoBot.Events;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot
{
    /// <summary>
    /// The AutoBot console
    /// </summary>
    public class AutoBotConsole : BaseCommandConsole
    {
        #region Dependencies

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Gets or sets the channel service.
        /// </summary>
        /// <value>
        /// The channel service.
        /// </value>
        public IChannelService ChannelService { get; set; }

        /// <summary>
        /// Gets or sets the handler service.
        /// </summary>
        /// <value>
        /// The handler service.
        /// </value>
        public IHandlerService HandlerService { get; set; }

        /// <summary>
        /// Gets or sets the credential service.
        /// </summary>
        /// <value>
        /// The credential service.
        /// </value>
        public ICredentialService CredentialService { get; set; }

        /// <summary>
        /// Gets or sets the file watcher service.
        /// </summary>
        /// <value>
        /// The file watcher service.
        /// </value>
        public IFileWatcherService FileWatcherService { get; set; }

        #endregion

        /// <summary>
        /// Main entry point for the console.
        /// </summary>
        protected override void Main()
        {
            ChatService.OnLogin += ChatService_OnLogin;
            ChatService.OnMessage += ChatService_OnMessage;

            base.Main();
        }

        /// <summary>
        /// Handles the OnMessage event of the ChatService.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        void ChatService_OnMessage(object sender, MessageEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(OnMessageCallback, e);
        }

        void OnMessageCallback(object parameters)
        {
            var e = (MessageEventArgs)parameters;

            HandlerService.Handle(e.Message);
        }

        /// <summary>
        /// Handles the OnLogin event of the ChatService.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        void ChatService_OnLogin(object sender, EventArgs e)
        {
            Thread.Sleep(4000);

            // Re-join channels
            ChannelService.Reconnect();

            // Continue watching any files
            FileWatcherService.Initialize();
        }

        /// <summary>
        /// The interactive HipBot command line.
        /// </summary>
        public override void Default()
        {
            Credentials credentials;

            var parameters = Parameters.Current;

            // Parse the command line
            if (parameters.Contains("load") && CredentialService.CredentialsSet())
            {
                credentials = CredentialService.GetCredentials();
            }
            else
            {
                credentials = CredentialService.ParseCredentials(parameters);
            }

            // Validate connection
            if (!CredentialService.Validate(credentials))
            {
                DisplayUsageMessage();

                return;
            }

            // Save credentials
            if (parameters.Contains("save"))
            {
                CredentialService.SetCredentials(credentials);
            }

            // Login
            ChatService.Login(credentials);

            // Wait forever if running as a console
            if (Environment.UserInteractive)
            {
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void DisplayUsageMessage()
        {
            Console.WriteLine("AutoBot IRC Bot");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("  autobot    -s [server] -p [port] -pwd [password] -n [nick] -r [real name]");
            Console.WriteLine("             (-load|-save) (-config [config]) (-install|-uninstall) ");
            Console.WriteLine("");
            Console.WriteLine("  -s         The IRC server hostname");
            Console.WriteLine("  -p         The IRC server port");
            Console.WriteLine("  -pwd       The IRC server password");
            Console.WriteLine("  -n         The nickname to use");
            Console.WriteLine("  -r         The realname to use");
            Console.WriteLine("  -save      Saves the given credentials (in plain text)");
            Console.WriteLine("  -load      Loads the previously saved credentials");
            Console.WriteLine("  -config    Use a custom configuration file name. The default is 'autobot'");
            Console.WriteLine("  -install   Installs Autobot as a Windows service");
            Console.WriteLine("  -uninstall Uninstalls Autobot as a Windows service");
        }
    }
}
