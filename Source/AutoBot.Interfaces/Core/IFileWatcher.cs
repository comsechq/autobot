using System.IO;

namespace AutoBot.Core
{
    /// <summary>
    /// Interface to wrap a SystemFileWatcher
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Watches the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        void Watch(string fileName);

        /// <summary>
        /// Occurs when the watched file changes.
        /// </summary>
        event FileSystemEventHandler Changed;
    }
}
