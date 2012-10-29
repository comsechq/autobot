using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Update
{
    /// <summary>
    /// Sets the update URL
    /// </summary>
    public class SetUpdateUrl : Handler<SetUpdateUrl.Options>
    {
        [Flag("update", "url")]
        public class Options
        {
            [Parameter("url")]
            public string Url { get; set; }
        }

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            if (!string.IsNullOrEmpty(options.Url))
            {
                ConfigService.SetValue("Update", "Url", options.Url);

                ChatService.Reply(message, "Update URL set.");
            }
            else
            {
                var url = ConfigService.GetValue("Update", "Url", string.Empty);

                ChatService.Reply(message, "Updating from URL: {0}", url);
            }
        }
    }
}
