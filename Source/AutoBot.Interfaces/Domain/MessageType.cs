namespace AutoBot.Domain
{
    /// <summary>
    /// Enumerates the types of IRC messages sent/recieved
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The message is of an unknown type
        /// </summary>
        Unknown,
        /// <summary>
        /// The message is a ping from the server
        /// </summary>
        Ping,
        /// <summary>
        /// The message is a private message
        /// </summary>
        PrivateMessage
    }
}
