using System.IO;

namespace AutoBot.Core
{
    /// <summary>
    /// Wrapper class for the <see cref="FileSystemWatcher"/> object.
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        private FileSystemWatcher watcher;

        /// <summary>
        /// Watches the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Watch(string fileName)
        {
            var path = Path.GetDirectoryName(fileName);
            var file = Path.GetFileName(fileName);

            watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Filter = file;

            watcher.Changed += watcher_Changed;

            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Handles the Changed event of the watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Changed(sender, e);
        }

        /// <summary>
        /// Occurs when the watched file changes.
        /// </summary>
        public event FileSystemEventHandler Changed;
    }
}
