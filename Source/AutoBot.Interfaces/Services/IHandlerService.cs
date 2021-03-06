﻿using AutoBot.Domain;

namespace AutoBot.Services
{
    /// <summary>
    /// Interface for handling incoming messages.
    /// </summary>
    public interface IHandlerService
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Handle(Message message);
    }
}
