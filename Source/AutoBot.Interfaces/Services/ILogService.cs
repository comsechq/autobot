using System.Collections.Generic;
using AutoBot.Domain;

namespace AutoBot.Services
{
    /// <summary>
    /// Service interface for IRC logging
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Determines whether logging is enabled.
        /// </summary>
        /// <returns></returns>
        bool LoggingEnabled();

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Log(Message message);

        /// <summary>
        /// Sets the logging enabled flag.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> enable logging.</param>
        void SetLoggingEnabled(bool enabled);

        /// <summary>
        /// Gets the log top number of lines.
        /// </summary>
        /// <param name="count">The lines.</param>
        /// <returns></returns>
        IList<string> GetLogTop(int count);

        IList<string> Search(string query);
    }
}
