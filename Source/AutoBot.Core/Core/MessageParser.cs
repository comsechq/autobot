using System.Collections.Generic;
using AutoBot.Domain;

namespace AutoBot.Core
{
    /// <summary>
    /// IRC Message Parser
    /// </summary>
    public class MessageParser : IMessageParser
    {
        #region Dependencies

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public IList<IMessageParserLink> Links { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageParser" /> class.
        /// </summary>
        public MessageParser()
        {
            Links = new List<IMessageParserLink>();
        }

        /// <summary>
        /// Parses the specified input into an IRC message.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Message Parse(string input)
        {
            // Default unknown message
            var message = new Message
            {
                Body = input,
                Type = MessageType.Unknown
            };

            // Process each parser link
            foreach (var link in Links)
            {
                if (link.CanParse(input))
                {
                    message = link.Parse(input);

                    break;
                }
            }

            return message;
        }
    }
}
