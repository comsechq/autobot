using System.Collections.Generic;
using System.Linq;
using AutoBot.Core;
using Sugar.Net;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to manipulate channels on the channels on the IRC server.
    /// </summary>
    public class ChannelService : IChannelService
    {
        private const string ChannelSectionName = "Channels";

        #region Dependencies

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

        #endregion

        /// <summary>
        /// Gets all the channels.
        /// </summary>
        /// <returns></returns>
        public IList<string> List()
        {
            var channels = ConfigService.GetValues(ChannelSectionName);

            return channels.Select(c => c.Key).ToList();
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
                ConfigService.SetValue(ChannelSectionName, name, string.Empty);
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

            ConfigService.DeleteValue(ChannelSectionName, name);

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

            var channels = ConfigService.GetValues(ChannelSectionName);

            foreach (var channel in channels)
            {
                Console.WriteLine("Rejoining: {0}", channel.Key);

                ChatService.Join(channel.Key);
            }
        }
    }
}
