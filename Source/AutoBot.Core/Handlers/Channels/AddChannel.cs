using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Channels
{
    /// <summary>
    /// Tells the Bot to join a Channel
    /// </summary>
    public class AddChannel : Handler<AddChannel.Options>
    {
        [Flag("channel")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name of the channel to add to the bot.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("add", Required = true)]
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
            ChannelService.Join(options.Channel);

            ChatService.ReplyFormat(message, "Added channel: {0}", options.Channel);
        }
    }
}
