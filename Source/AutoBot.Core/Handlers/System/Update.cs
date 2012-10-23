using System.ComponentModel.Composition;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays the current processes running
    /// </summary>
    [Export(typeof(IHandler))]
    public class Update : Handler<Update.Options>
    {
        [Flag("update")]
        public class Options {}

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Gets or sets the update service.
        /// </summary>
        /// <value>
        /// The update service.
        /// </value>
        public IUpdateService UpdateService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            if (!UpdateService.IsNewVersionAvailable())
            {
                ChatService.Reply(message, "No new version is available.");
            }
            else
            {
                ChatService.Reply(message, "Downloading new version of bot...");

                UpdateService.DownloadUpdate();

                ChatService.Reply(message, "New version downloaded.  Recycle me to upgrade.");                
            }
        }
    }
}
