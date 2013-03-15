using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Logging
{
    /// <summary>
    /// Adds a nickname to this bot
    /// </summary>
    public class EnableLogging : Handler<EnableLogging.Options>
    {
        [Flag("log")]
        public class Options
        {
            [Flag("on")]
            public bool On { get; set; }

            [Flag("off")]
            public bool Off { get; set; }
        }

        #region Dependencies

        /// <summary>
        /// Gets or sets the log service.
        /// </summary>
        /// <value>
        /// The log service.
        /// </value>
        public ILogService LogService { get; set; }

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
            if (options.On)
            {
                LogService.SetLoggingEnabled(true);

                ChatService.Reply(message, "Logging: on");                
            }
            else if (options.Off)
            {
                LogService.SetLoggingEnabled(false);

                ChatService.Reply(message, "Logging: off");
            }
        }
    }
}
