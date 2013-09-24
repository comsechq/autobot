using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Watches
{
    /// <summary>
    /// Tells the bot to stop watching a file
    /// </summary>
    public class RemoveWatch : Handler<RemoveWatch.Options>
    {
        [Flag("watch")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name for the file.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("remove", Required = true)]
            public string FileName { get; set; }
        }

        #region Dependencies

        /// <summary>
        /// Gets or sets the file watcher service.
        /// </summary>
        /// <value>
        /// The file watcher service.
        /// </value>
        public IFileWatcherService FileWatcherService { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            FileWatcherService.RemoveWatch(options.FileName);
        }
    }
}
