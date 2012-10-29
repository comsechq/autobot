namespace AutoBot.Core
{
    /// <summary>
    /// Interface for outputing system messages from the bot.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Writes the given value to the console.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void WriteLine(string format, params object[] args);
    }
}
