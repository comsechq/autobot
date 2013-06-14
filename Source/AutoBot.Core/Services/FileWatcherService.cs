using System.IO;
using AutoBot.Core;
using AutoBot.Domain;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to watch for files and display their contents
    /// </summary>
    public class FileWatcherService : IFileWatcherService
    {
        private string notificationChannel;

        #region Dependencies
        
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
        /// Gets or sets the file watcher.
        /// </summary>
        /// <value>
        /// The file watcher.
        /// </value>
        public IFileWatcher FileWatcher { get; set; }

        #endregion

        /// <summary>
        /// Initializes the service with any previous file
        /// being watched.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Initialize()
        {
            var fileName = ConfigService.GetValue("watch", "file", "");

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            notificationChannel = ConfigService.GetValue("watch", "channel", "");

            Watch(fileName, notificationChannel);
        }

        /// <summary>
        /// Watches the specified file name and chat's it's contents.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="channel">The channel to display the contents of the watched file.</param>
        public void Watch(string fileName, string channel)
        {
            ConfigService.SetValue("watch", "file", fileName);
            ConfigService.SetValue("watch", "channel", channel);

            notificationChannel = channel;

            FileWatcher.Watch(fileName);

            FileWatcher.Changed += FileWatcher_Changed;
        }

        void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!File.Exists(e.FullPath)) return;

                var contents = File.ReadAllText(e.FullPath);

                var message = new Message();
                message.To = notificationChannel;
                message.From = "#test";
                message.Type = MessageType.Unknown;
                message.UserHost = "";

                ChatService.Reply(message, contents);

                File.Delete(e.FullPath);
            }
            catch
            {                
            }
        }
    }
}
