using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Aliases
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class ListAlias : Handler<ListAlias.Options>
    {
        [Flag("alias", "list")]
        public class Options {}

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the alias service.
        /// </summary>
        /// <value>
        /// The nickname service.
        /// </value>
        public IAliasService AliasService { get; set; }

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
            var aliases = AliasService.ListAliases();

            foreach (var alias in aliases)
            {
                ChatService.Reply(message, alias);
            }

            ChatService.Reply(message, "{0} aliases.", aliases.Count);
        }
    }
}
