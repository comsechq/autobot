using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoBot.Core;
using AutoBot.Domain;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to watch for files and display their contents
    /// </summary>
    public class FileWatcherService : IFileWatcherService
    {
        private const string FileWatcherSectionName = "watch";

        /// <summary>
        /// Gets the file watches.
        /// </summary>
        /// <value>
        /// The file watches.
        /// </value>
        public IList<IFileWatcher> FileWatchers { get; private set; }

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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatcherService"/> class.
        /// </summary>
        public FileWatcherService()
        {
            FileWatchers = new List<IFileWatcher>();
        }

        /// <summary>
        /// Initializes the service with any previous file
        /// being watched.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Initialize()
        {
            var watches = ConfigService.GetValues(FileWatcherSectionName);

            foreach (var watch in watches)
            {
                AddFileWatcher(watch.Key, watch.Value);
            }
        }

        /// <summary>
        /// Watches the specified file name and chat's it's contents.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="channel">The channel to display the contents of the watched file.</param>
        public void AddWatch(string fileName, string channel)
        {
            ConfigService.SetValue(FileWatcherSectionName, fileName, channel);

            AddFileWatcher(fileName, channel);
        }

        /// <summary>
        /// Stops watching the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveWatch(string fileName)
        {
            for (var i = FileWatchers.Count - 1; i >= 0; i--)
            {
                var match = string.Compare(fileName, FileWatchers[i].FileName, StringComparison.InvariantCultureIgnoreCase) == 0;

                if (!match) continue;

                FileWatchers.RemoveAt(i);

                ConfigService.DeleteValue(FileWatcherSectionName, fileName);
            }
        }

        /// <summary>
        /// Adds the file watcher to the FileWatcher colleciton
        /// and associates it's change event with the service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="channel">The channel.</param>
        private void AddFileWatcher(string fileName, string channel)
        {
            var fileWatcher = new FileWatcher(fileName, channel);

            fileWatcher.Changed += FileChanged;

            FileWatchers.Add(fileWatcher);
        }

        /// <summary>
        /// Handles the Changed event of the FileWatcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        public void FileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                var fullPath = e.FullPath;

                // Stip leading slash
                if (fullPath.StartsWith("\\"))
                {
                    fullPath = fullPath.Substring(1);
                }

                if (!File.Exists(fullPath)) return;

                var contents = File.ReadAllText(fullPath);

                var notificationChannel = FileWatchers
                    .First(w => w.FileName == fullPath)
                    .Channel;

                var message = new Message();
                message.To = notificationChannel;
                message.From = notificationChannel;
                message.Type = MessageType.Unknown;
                message.UserHost = "";

                ChatService.Reply(message, contents);

                File.Delete(fullPath);
            }
            catch
            {            
            }
        }
    }
}
