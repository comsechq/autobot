using System.IO;

namespace AutoBot.Core
{
    /// <summary>
    /// Wrapper class for the <see cref="FileSystemWatcher"/> object.
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        private readonly FileSystemWatcher watcher;

        /// <summary>
        /// Watches the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="channel">The channel.</param>
        public FileWatcher(string fileName, string channel)
        {
            FileName = fileName;
            Channel = channel;

            var path = Path.GetDirectoryName(fileName);
            var file = Path.GetFileName(fileName);

            watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Filter = file;

            watcher.Changed += watcher_Changed;

            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Gets the name of the file to watch.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the IRC channel to log contents of the file to.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public string Channel { get; private set; }

        /// <summary>
        /// Handles the Changed event of the watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (Changed != null) Changed(sender, e);
        }

        /// <summary>
        /// Occurs when the watched file changes.
        /// </summary>
        public event FileSystemEventHandler Changed;
    }
}
