﻿using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Channels
{
    /// <summary>
    /// Removes a channel from the bot
    /// </summary>
    public class RemoveChannel : Handler<RemoveChannel.Options>
    {
        [Flag("channel")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("remove", Required = true)]
            public string Channel { get; set; }
        }

        #region Dependencies

        /// <summary>
        /// Gets or sets the channel service.
        /// </summary>
        /// <value>
        /// The channel service.
        /// </value>
        public IChannelService ChannelService { get; set; }

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            ChannelService.Leave(options.Channel);

            ChatService.ReplyFormat(message, "Removed channel: {0}", options.Channel);
        }
    }
}
