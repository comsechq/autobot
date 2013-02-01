﻿using System.Collections.Generic;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Handlers;
using Sugar;

namespace AutoBot.Services
{
    /// <summary>
    /// Service for handling incoming messages
    /// </summary>
    public class HandlerService : IHandlerService
    {
        #region Dependencies

        /// <summary>
        /// Gets or sets the handlers.
        /// </summary>
        /// <value>
        /// The handlers.
        /// </value>
        public IList<IHandler> Handlers { get; set; }

        /// <summary>
        /// Gets or sets the nickname service.
        /// </summary>
        /// <value>
        /// The nickname service.
        /// </value>
        public INicknameService NicknameService { get; set; }

        /// <summary>
        /// Gets or sets the alias service.
        /// </summary>
        /// <value>
        /// The alias service.
        /// </value>
        public IAliasService AliasService { get; set; }

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerService"/> class.
        /// </summary>
        public HandlerService()
        {
            Handlers = new List<IHandler>();
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(Message message)
        {          
            // Check bot can handle this message
            if (!CanHandle(message))
            {
                return;
            }

            // Check for aliases
            var command = message.Body.SubstringBeforeChar(" ");
            if (AliasService.IsAlias(command))
            {
                message.Body = AliasService.GetAlias(command) + " " + message.Body.SubstringAfterChar(" ");
            }

            // Remove Alias bypass
            if (message.Body.StartsWith("!"))
            {
                message.Body = message.Body.Substring(1);
            }

            var handled = false;

            // Check each handler
            foreach (var handler in Handlers)
            {
                if (!handler.CanHandle(message)) continue;

                handler.Receive(message);

                handled = true;

                break;
            }

            if (!handled)
            {
                ChatService.Reply(message, string.Format("I didn't understand: {0}", message.Body));
            }
        }

        /// <summary>
        /// Determines whether this instance can handle the specified message in the room.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified room; otherwise, <c>false</c>.
        /// </returns>
        private bool CanHandle(Message message)
        {
            var canHandle = false;

            // Check message is not one-on-one message
            if (message.Type == MessageType.PrivateMessage)
            {
                // Always accept if a DM
                if (message.To == Context.Nick)
                {
                    canHandle = true;
                }

                // Check message is addressed to this bot
                else if (NicknameService.IsAddressedToMe(message))
                {
                    canHandle = true;

                    // Strip bot name from message
                    message.Body = message.Body.SubstringAfterChar(" ");
                }
            }

            return canHandle;
        }
    }
}
