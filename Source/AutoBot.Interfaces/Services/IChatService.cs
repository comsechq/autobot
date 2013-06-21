using System;
using AutoBot.Domain;
using AutoBot.Events;

namespace AutoBot.Services
{
    /// <summary>
    /// Interface for accessing the IRC network.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Occurs when the Bot logs in
        /// </summary>
        event EventHandler<LoginEventArgs> OnLogin;

        /// <summary>
        /// Occurs when the Bot receives a message
        /// </summary>
        event EventHandler<MessageEventArgs> OnMessage;

        /// <summary>
        /// Logs in with the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        void Login(Credentials credentials);

        /// <summary>
        /// Gets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logged in; otherwise, <c>false</c>.
        /// </value>
        bool LoggedIn { get; }

        /// <summary>
        /// Joins the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns></returns>
        bool Join(string channel);

        /// <summary>
        /// Leaves the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        bool Leave(string channel);

        /// <summary>
        /// Replies to the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="response">The response.</param>
        void Reply(string channel, string response);

        /// <summary>
        /// Replies to the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="response">The response.</param>
        /// <param name="args">The args.</param>
        void ReplyFormat(string channel, string response, params object[] args);

        /// <summary>
        /// Replies the specified message with the given response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The response.</param>
        void Reply(Message message, string response);

        /// <summary>
        /// Replies the specified message with the given response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The response.</param>
        /// <param name="args">The args.</param>
        void ReplyFormat(Message message, string response, params object[] args);
    }
}
