using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Watches
{
    /// <summary>
    /// Lists all the files the bot is watching
    /// </summary>
    public class ListWatches : Handler<ListWatches.Options>
    {
        [Flag("watch", "list")]
        public class Options
        {
        }

        #region Dependencies

        /// <summary>
        /// Gets or sets the file watcher service.
        /// </summary>
        /// <value>
        /// The file watcher service.
        /// </value>
        public IFileWatcherService FileWatcherService { get; set; }

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
            foreach (var watcher in FileWatcherService.FileWatchers)
            {
                ChatService.Reply(message, watcher.FileName);
            }
        }
    }
}
