﻿using AutoBot.Domain;

namespace AutoBot.Handlers
{
    /// <summary>
    /// Interface to define the handling of incoming messages
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Determines whether this instance can handle the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified message; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(Message message);

        /// <summary>
        /// Occurs when a message is received from the given channel.
        /// </summary>
        /// <param name="message">The message.</param>
        void Receive(Message message);
    }
}
