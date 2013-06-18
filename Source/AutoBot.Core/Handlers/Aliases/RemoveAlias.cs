using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Aliases
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class RemoveAlias : Handler<RemoveAlias.Options>
    {
        [Flag("alias", "remove")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("remove", Required = true)]
            public string Name { get; set; }
        }

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
            AliasService.RemoveAlias(options.Name);

            ChatService.Reply(message, "Removed alias '{0}'.", options.Name);
        }
    }
}
