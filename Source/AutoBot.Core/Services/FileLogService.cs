using System;
using System.Collections.Generic;
using System.IO;
using AutoBot.Domain;
using Sugar;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to log IRC messages
    /// </summary>
    public class FileLogService : ILogService
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        #endregion

        private string GetLogFileName(DateTime dateTime)
        {
            var rootPath = ConfigService.GetValue("logging", "path", "C:\\logs");
            var datePath = string.Format("{0:yyyy}\\{0:MM}\\{0:dd}", dateTime);

            return Path.Combine(rootPath, datePath, "irc-log.txt");
        }

        /// <summary>
        /// Determines whether logging is enabled.
        /// </summary>
        /// <returns></returns>
        public bool LoggingEnabled()
        {
            var enabled = ConfigService.GetValue("logging", "enabled", "false");

            return enabled == "true";
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(Message message)
        {
            // Discard system messages
            if (string.IsNullOrEmpty(message.To))
            {
                return;
            }

            var now = DateTime.Now;
            var fileName = GetLogFileName(now);
            var path = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var timestamp = string.Format("{0:yyyy-MM-dd HH:mm:ss} [{1}] {2}: {3}{4}", now, message.To, message.From, message.Body, Environment.NewLine);

            try
            {
                File.AppendAllText(fileName, timestamp);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Sets the logging enabled flag.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> enable logging.</param>
        public void SetLoggingEnabled(bool enabled)
        {
            var config = ConfigService.GetConfig();

            config.SetValue("logging", "enabled", enabled.ToString().ToLower());

            ConfigService.SetConfig(config);
        }

        /// <summary>
        /// Gets the log top number of lines.
        /// </summary>
        /// <param name="count">The lines.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<string> GetLogTop(int count)
        {
            var path = GetLogFileName(DateTime.Now);

            var lines = File.ReadAllLines(path);
            var results = new List<string>();

            var c = 0;

            for (var i = lines.Length - 1; i >= 0; i--)
            {
                if (c > count) break;

                results.Add(lines[i]);

                c++;
            }

            return results;
        }

        /// <summary>
        /// Searches the logs for the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<string> Search(string query)
        {
            var results = new List<string>();

            var dates = DateTime.Now.AddDays(-10).DaysUntil(DateTime.Now);

            foreach (var date in dates)
            {
                results.AddRange(Search(query, date));
            }

            return results;
        }

        /// <summary>
        /// Searches the logs for the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public IList<string> Search(string query, DateTime dateTime)
        {
            var results = new List<string>();
            
            var path = GetLogFileName(dateTime);

            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);

                foreach (var line in lines)
                {
                    if (line.Contains(query))
                    {
                        results.Add(line);
                    }
                }
            }

            return results;
        }
    }
}
