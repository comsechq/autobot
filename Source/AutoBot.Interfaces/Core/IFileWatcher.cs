using System.IO;

namespace AutoBot.Core
{
    /// <summary>
    /// Interface to wrap a SystemFileWatcher
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Gets the name of the file to watch.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        string FileName { get; }

        /// <summary>
        /// Gets the IRC channel to log contents of the file to.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        string Channel { get; }

        /// <summary>
        /// Occurs when the watched file changes.
        /// </summary>
        event FileSystemEventHandler Changed;
    }
}
