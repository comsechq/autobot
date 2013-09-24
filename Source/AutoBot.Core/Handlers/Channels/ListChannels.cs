using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Channels
{
    /// <summary>
    /// Lists the channels from the bot has joined
    /// </summary>
    public class ListChannels : Handler<ListChannels.Options>
    {
        [Flag("channel", "list")]
        public class Options
        {
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
            var channels = ChannelService.List();

            foreach (var channel in channels)
            {
                ChatService.ReplyFormat(message, channel);
            }
        }
    }
}
