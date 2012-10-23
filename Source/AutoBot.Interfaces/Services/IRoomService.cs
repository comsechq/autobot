using System.Collections.Generic;

namespace AutoBot.Services
{
    /// <summary>
    /// Service interface for IRC rooms.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Gets all the rooms.
        /// </summary>
        /// <returns></returns>
        IList<string> List();

        /// <summary>
        /// Joins the room with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Join(string name);

        /// <summary>
        /// Leaves the room with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool Leave(string name);

        /// <summary>
        /// Reconnects all rooms on login.
        /// </summary>
        void Reconnect();
    }
}
