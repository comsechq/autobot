﻿using System;
using AutoBot.Domain;
using Sugar;

namespace AutoBot.Core.MessageParserLinks
{
    /// <summary>
    /// Parser link to handle PING messages
    /// </summary>
    public class PingLink : IMessageParserLink
    {
        /// <summary>
        /// Determines whether this instance can parse the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if this instance can parse the specified input; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool CanParse(string input)
        {
            var result = false;

            if (!string.IsNullOrEmpty(input))
            {
                result = input.StartsWith("PING ");
            }

            return result;
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Message Parse(string input)
        {
            var message = new Message
            {
                Received = DateTime.Now,
                Body = input.SubstringAfterChar(" "),
                Type = MessageType.Ping
            };

            return message;
        }
    }
}
