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
        void Watch(string fileName, string channel);
    }
}
