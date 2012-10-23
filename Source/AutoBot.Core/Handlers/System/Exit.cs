using System;
using System.Threading;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Says hello to the sender
    /// </summary>
    public class Exit : Handler<Exit.Options>
    {
        [Flag("exit")]
        public class Options {}

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            ChatService.Reply(message, "Exiting bot.");

            Thread.Sleep(100);

            Environment.Exit(0);
        }
    }
}
