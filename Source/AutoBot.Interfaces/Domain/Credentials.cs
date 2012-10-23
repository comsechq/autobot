namespace AutoBot.Domain
{
    /// <summary>
    /// Represents a set of credentials used to connect to an IRC server.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Gets or sets the server hostname that the IRC is running on.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the port the IRC server is running on.
        /// </summary>
        /// <value>
        /// The port number.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the password required to access the IRC server.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the nick to be used for the IRC connection.
        /// </summary>
        /// <value>
        /// The nick.
        /// </value>
        public string Nick { get; set; }

        /// <summary>
        /// Gets or sets the real name to use for the IRC connection.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
