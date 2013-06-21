using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Nicknames
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class RemoveNickname : Handler<RemoveNickname.Options>
    {
        [Flag("nick")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("remove", Required = true)]
            public string Nickname { get; set; }
        }

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the nickname service.
        /// </summary>
        /// <value>
        /// The nickname service.
        /// </value>
        public INicknameService NicknameService { get; set; }

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
            NicknameService.Remove(options.Nickname);

            ChatService.ReplyFormat(message, "Removed nickname '{0}'.", options.Nickname);
        }
    }
}
