﻿using System.IO;
using Sugar.Command;
using Sugar.Configuration;
using Sugar.IO;

namespace AutoBot.Services
{
    /// <summary>
    /// Wrapper for accessing the configuration
    /// </summary>
    public class ConfigService : IConfigService
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IFileService FileService { get; set; }

        #endregion

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public Config GetConfig()
        {
            // Get file location
            var filename = GetConfigurationFilename();

            // Load the configuration data
            return Config.FromFile(filename, FileService);
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The configuration to set.</param>
        public void SetConfig(Config config)
        {
            config.Write(GetConfigurationFilename());
        }

        /// <summary>
        /// Gets the value from the given key in the configuration.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetValue(string section, string key, string @default)
        {
            var config = GetConfig();

            return config.GetValue(section, key, @default);
        }

        /// <summary>
        /// Sets the value with the given key in the configuration.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetValue(string section, string key, object value)
        {
            var config = GetConfig();

            config.SetValue(section, key, value.ToString());

            SetConfig(config);
        }

        private string GetConfigurationFilename()
        {
            // Configuration file in user directory
            var directory = Path.GetDirectoryName(typeof(ConfigService).Assembly.Location) ?? string.Empty;

            // Ensure directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Check for custom config filename
            var configFileName = "AutoBot.config";
            if (Parameters.Current.Contains("config"))
            {
                configFileName = string.Format("{0}.config", Parameters.Current.AsString("config"));
            }

            return Path.Combine(directory, configFileName);
        }
    }
}
