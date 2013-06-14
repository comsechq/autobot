using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Echo's the given input
    /// </summary>
    public class Echo : Handler<Echo.Options>
    {
        [Flag("echo")]
        public class Options
        {
            [Parameter("echo")]
            public string Message { get; set; }
        }

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            ChatService.Reply(message, options.Message);
        }
    }
}
