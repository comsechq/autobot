﻿using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.FileWatcher
{
    /// <summary>
    /// Tells the bot to watch a file
    /// </summary>
    public class Watch : Handler<Watch.Options>
    {
        [Flag("watch")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [Parameter("watch", Required = true)]
            public string FileName { get; set; }

            [Parameter("channel", Required = true)]
            public string Channel { get; set; }
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
            FileWatcherService.Watch(options.FileName, options.Channel);

            ChatService.Reply(message, "Watching: {0}", options.FileName);
        }
    }
}