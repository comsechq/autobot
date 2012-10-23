using AutoBot.Domain;

namespace AutoBot.Core
{
    /// <summary>
    /// Interface for an IRC message parsing class
    /// </summary>
    public interface IMessageParser
    {
        /// <summary>
        /// Parses the specified input into an IRC message.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Message Parse(string input);
    }
}
