using AutoBot.Domain;

namespace AutoBot.Core
{
    /// <summary>
    /// An inidividual IRC message parser for use with the <see cref="IMessageParser"/>
    /// </summary>
    public interface IMessageParserLink
    {
        /// <summary>
        /// Determines whether this instance can parse the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if this instance can parse the specified input; otherwise, <c>false</c>.
        /// </returns>
        bool CanParse(string input);

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Message Parse(string input);
    }
}
