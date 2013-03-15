using System;
using Con = System.Console;

namespace AutoBot.Core
{
    /// <summary>
    /// System Console
    /// </summary>
    public class Console : IConsole
    {
        /// <summary>
        /// Writes the given value to the console.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WriteLine(string format, params object[] args)
        {
            try
            {
                Con.WriteLine(format, args);
            }
            catch (Exception)
            {
                Con.WriteLine(format);
            }
        }
    }
}
