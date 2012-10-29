namespace AutoBot.Services
{
    /// <summary>
    /// Interface for the Automatic Update service
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// Gets the update URL to retrieve the latest version from.
        /// </summary>
        /// <returns></returns>
        string GetUpdateUrl();

        /// <summary>
        /// Sets the update URL to the given value.
        /// </summary>
        /// <param name="url">The URL.</param>
        void SetUpdateUrl(string url);

        /// <summary>
        /// Gets the version directory from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        string GetVersionDirectoryFromUrl(string url);

        /// <summary>
        /// Determines whether if a new version of the Bot is available.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if a new version is available; otherwise, <c>false</c>.
        /// </returns>
        bool IsNewVersionAvailable();

        /// <summary>
        /// Downloads the latest version of the bot from the Update server.
        /// </summary>
        string DownloadUpdate();

        /// <summary>
        /// Removes the current version of the Bot.
        /// </summary>
        void RemoveCurrentVersion();

        void CopyUpdate(string path);

        void RemoveUpdate(string path);

        void LaunchBot();
    }
}
