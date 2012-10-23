using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Nicknames
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class ListNickname : Handler<ListNickname.Options>
    {
        [Flag("nick", "list")]
        public class Options { }

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
            ChatService.Reply(message, "I answer to the following names:");

            foreach (var name in NicknameService.List())
            {
                ChatService.Reply(message, name);
            }
        }
    }
}
