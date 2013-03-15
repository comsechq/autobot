using System.Collections.Generic;

namespace AutoBot.Services
{
    /// <summary>
    /// Service interface for IRC channels.
    /// </summary>
    public interface IChannelService
    {
        /// <summary>
        /// Gets all the channels.
        /// </summary>
        /// <returns></returns>
        IList<string> List();

        /// <summary>
        /// Joins the channel with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Join(string name);

        /// <summary>
        /// Leaves the channel with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Leave(string name);

        /// <summary>
        /// Reconnects all channels on login.
        /// </summary>
        void Reconnect();
    }
}
