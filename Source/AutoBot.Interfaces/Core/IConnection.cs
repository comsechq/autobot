using System;

namespace AutoBot
{
    /// <summary>
    /// Interface for a connection to an IRC server
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Connects the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        bool Connect(string server, int port);

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Send(string data);

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="args">The args.</param>
        void Send(string data, params object[] args);

        /// <summary>
        /// Receives some data from the server.
        /// </summary>
        /// <returns></returns>
        string Receive();
    }
}
