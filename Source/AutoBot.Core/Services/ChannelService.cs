using System.Collections.Generic;
using AutoBot.Core;
using Sugar.Net;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to manipulate channels on the channels on the IRC server.
    /// </summary>
    public class ChannelService : IChannelService 
    {
        /// <summary>
        /// Gets or sets the HTTP service.
        /// </summary>
        /// <value>
        /// The HTTP service.
        /// </value>
        public IHttpService HttpService { get; set; }

        /// <summary>
        /// Gets or sets the credential service.
        /// </summary>
        /// <value>
        /// The credential service.
        /// </value>
        public ICredentialService CredentialService { get; set; }

        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        public IConsole Console { get; set; }

        /// <summary>
        /// Gets all the channels.
        /// </summary>
        /// <returns></returns>
        public IList<string> List()
        {
            var channels = new List<string>();

           

            return channels;
        }

        /// <summary>
        /// Joins the channel with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool Join(string name)
        {
            var joined = ChatService.Join(name);

            if (joined)
            {
                var config = ConfigService.GetConfig();

                config.SetValue("Channels", name, string.Empty);

                ConfigService.SetConfig(config);
            }
         
            return joined;
        }

        /// <summary>
        /// Leaves the channel with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Leave(string name)
        {
            var left = ChatService.Leave(name);

            if (left)
            {
                var config = ConfigService.GetConfig();

                config.Delete("Channels", name);

                ConfigService.SetConfig(config);
            }

            return left;
        }

        /// <summary>
        /// Reconnects all channels on login.
        /// </summary>
        public void Reconnect()
        {
            if (!ChatService.LoggedIn)
            {
                return;                
            }

            var config = ConfigService.GetConfig();

            var lines = config.GetSection("Channels");

            foreach (var line in lines)
            {
                Console.WriteLine("Attempting to rejoin channel {0}", line.Key);

                ChatService.Join(line.Key);
            }

            ConfigService.SetConfig(config);
        }
    }
}
