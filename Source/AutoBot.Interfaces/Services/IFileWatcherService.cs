using System.Collections.Generic;
using AutoBot.Core;

namespace AutoBot.Services
{
    /// <summary>
    /// Service interface for watching files.
    /// </summary>
    public interface IFileWatcherService
    {
        /// <summary>
        /// Initializes the service with any previous file
        /// being watched.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Watches the specified file name and chat's it's contents.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="channel">The channel to display the contents of the watched file.</param>
        void AddWatch(string fileName, string channel);

        /// <summary>
        /// Stops watching the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        void RemoveWatch(string fileName);

        /// <summary>
        /// Lists all the files being watched.
        /// </summary>
        /// <returns></returns>
        IList<IFileWatcher> FileWatchers { get; }
    }
}
