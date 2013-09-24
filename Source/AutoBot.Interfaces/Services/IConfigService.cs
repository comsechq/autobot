using System.Collections.Generic;
using System.Linq;
using Sugar.Configuration;

namespace AutoBot.Services
{
    /// <summary>
    /// Service interface for the Configuration.
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        Config GetConfig();

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="config">The configuration to set.</param>
        void SetConfig(Config config);

        /// <summary>
        /// Gets the value from the given key in the configuration.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        string GetValue(string section, string key, string @default);

        /// <summary>
        /// Gets all the values from a config section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        IList<ConfigLine> GetValues(string section);

        /// <summary>
        /// Sets the value with the given key in the configuration.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetValue(string section, string key, object value);

        /// <summary>
        /// Deletes the value with the given key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        void DeleteValue(string section, string key);
    }
}
