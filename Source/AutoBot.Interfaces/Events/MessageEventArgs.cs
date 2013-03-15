using System;
using AutoBot.Domain;

namespace AutoBot.Events
{
    /// <summary>
    /// Event arguments for when a message is received.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public Message Message { get; set; }

        /// <summary>
        /// Gets or sets the channel this message was recieved from.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public Channel Channel { get; set; }
    }
}
