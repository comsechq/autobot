using System;
using System.Diagnostics;
using System.IO;
using SevenZipLib;
using Sugar;
using Sugar.Command;
using Sugar.IO;
using Sugar.Net;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to handle to automatic updating of the Bot.
    /// </summary>
    public class UpdateService : IUpdateService
    {
        #region Dependencies

        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IFileService FileService { get; set; }

        /// <summary>
        /// Gets or sets the HTTP service.
        /// </summary>
        /// <value>
        /// The HTTP service.
        /// </value>
        public IHttpService HttpService { get; set; }

        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        #endregion

        public string GetUpdateUrl()
        {
            // This can be overridden in the config file
            const string defaultUrl = "https://raw.github.com/comsechq/autobot/master/version.txt";

            return ConfigService.GetValue("Update", "Url", defaultUrl);
        }

        /// <summary>
        /// Sets the update URL to the given value.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void SetUpdateUrl(string url)
        {
            ConfigService.SetValue("Update", "Url", url);
        }

        /// <summary>
        /// Gets the version directory from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public string GetVersionDirectoryFromUrl(string url)
        {
            string result;

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var uri = new Uri(url);

                var path = uri.AbsolutePath;

                var fileName = Path.GetFileName(path);

                // Strip extension
                fileName = fileName.SubstringBeforeLastChar(".");

                // Get last filename component
                fileName = fileName.SubstringAfterLastChar(".");

                var userDirectory = FileService.GetUserDataDirectory();

                // Combine paths
                result = Path.Combine(userDirectory, "autobot", fileName);
            }
            else
            {
                throw new ApplicationException("Invalid update URL: " + url);
            }

            return result;
        }

        /// <summary>
        /// Determines whether if a new version of the Bot is available.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if a new version is available; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewVersionAvailable()
        {
            Console.WriteLine("Checking for new AutoBot Version...");

            var current = ConfigService.GetValue("Update", "Current", string.Empty);
            var latest = GetLatestVersionUrl();

            // New version available?
            var newVersionAvailable = current != latest;

            if (newVersionAvailable)
            {
                Console.WriteLine("New Version Available.");
            }
            else
            {
                Console.WriteLine("AutoBot is up to date.");
            }

            return newVersionAvailable;
        }

        /// <summary>
        /// Gets the URL where the latest version is hosted.
        /// This is a text file with the URL of ZIP file containing the latest binaries. 
        /// </summary>
        /// <returns></returns>
        private string GetLatestVersionUrl()
        {
            var url = string.Empty;

            var response = HttpService.Get(GetUpdateUrl());

            if (response.Success)
            {
                url = response.ToString().Trim();
            }

            return url;
        }

        /// <summary>
        /// Downloads the latest version of the bot from the Update server.
        /// </summary>
        public string DownloadUpdate()
        {
            var url = GetLatestVersionUrl();

            var response = HttpService.Get(url);

            var path = GetVersionDirectoryFromUrl(url);

            if (response.Success)
            {
                var fileName = Path.Combine(path, Path.GetFileName(url) ?? "update.zip");

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                File.WriteAllBytes(fileName, response.Bytes);

                UncompressFile(fileName);

                ConfigService.SetValue("Update", "Current", url);
            }

            return path;
        }

        private void UncompressFile(string archiveFilename)
        {
            if (!File.Exists(archiveFilename))
            {
                Console.WriteLine("Can't Decompress: {0}", archiveFilename);
            }

            Console.WriteLine("Decompressing: {0}", archiveFilename);

            var path = Path.GetDirectoryName(archiveFilename) ?? string.Empty;

            using (var archive = new SevenZipArchive(archiveFilename))
            {
                foreach (var entry in archive)
                {
                    if (entry.IsDirectory) continue;

                    var filename = entry.FileName.SubstringAfterChar("\\");

                    Console.WriteLine("Extracting File: {0}", filename);

                    var filePath = Path.Combine(path, filename);

                    using (var stream = File.Create(filePath))
                    {
                        entry.Extract(stream, ExtractOptions.OverwriteExistingFiles);

                        stream.Flush();

                        stream.Close();
                    }
                }
            }

            File.Delete(archiveFilename);
        }

        /// <summary>
        /// Gets the install directory.
        /// </summary>
        /// <value>
        /// The install directory.
        /// </value>
        public string InstallDirectory
        {
            get
            {
                return Parameters.Directory + "\\..";               
            }
        }

        /// <summary>
        /// Removes the current version of the Bot.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveCurrentVersion()
        {
            var files = new[] { "autobot.core.dll", "autobot.exe", "autobot.handlers.dll", "autobot.interfaces.dll", "Sugar.dll"};

            FileService.Delete(InstallDirectory, files);
        }

        /// <summary>
        /// Copies the update to the installation directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CopyUpdate(string path)
        {
            FileService.Copy(path, InstallDirectory);
        }

        /// <summary>
        /// Removes the update.
        /// </summary>
        /// <param name="path">The path.</param>
        public void RemoveUpdate(string path)
        {
            FileService.Delete(path);
        }

        /// <summary>
        /// Launches the bot.
        /// </summary>
        public void LaunchBot()
        {
            var basePath = InstallDirectory;

            var botFilename = Path.Combine(basePath, "autobot.exe");

            var paramters = Parameters.Current;

            var info = new ProcessStartInfo
            {
                FileName = botFilename,
                UseShellExecute = false,
                Arguments = paramters.ToString()
            };

            Process.Start(info);

            Environment.Exit(0);
        }        
    }
}
