using System;
using System.Collections.Generic;

namespace AutoBot.Core
{
    /// <summary>
    /// Wrapper interface for executing a process.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Starts a new process using the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="args">The args.</param>
        void Start(string fileName, string args);

        /// <summary>
        /// Kills the started process.
        /// </summary>
        void Kill();

        /// <summary>
        /// Returns the process output.
        /// </summary>
        /// <returns></returns>
        IList<string> Output { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IProcess"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if executing; otherwise, <c>false</c>.
        /// </value>
        bool Running { get; }

        /// <summary>
        /// Gets the elapsed time since the process was started.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        TimeSpan ElapsedTime { get; }

        /// <summary>
        /// Gets the file name of the process.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        string FileName { get; }

        /// <summary>
        /// Occurs when the process exits.
        /// </summary>
        /// <returns></returns>
        event EventHandler Exited;
    }
}
