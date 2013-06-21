using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sugar;
using Prc = System.Diagnostics.Process;
using System.Collections.Generic;

namespace AutoBot.Core
{
    /// <summary>
    /// Wrapper class for launching processes
    /// </summary>
    public class Process : IProcess
    {
        private List<string> output;
        private DateTime startTime;
        private bool running;
        private Prc process;

        /// <summary>
        /// Initializes a new instance of the <see cref="Process"/> class.
        /// </summary>
        public Process()
        {
            output = new List<string>();
            running = false;
        }

        /// <summary>
        /// Starts a new process using the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="args">The args.</param>
        /// <exception cref="System.ApplicationException">Another process is already running!</exception>
        public void Start(string fileName, string args)
        {
            if (Running)
            {
                throw new ApplicationException("Another process is already running!");
            }

            startTime = DateTime.Now;
            output.Clear();
            FileName = fileName;

            process = new Prc
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = fileName,
                    Arguments = args
                }
            };

            process.Exited += ProcessExited;
            process.OutputDataReceived += ProcessOnOutputDataReceived;

            try
            {
                process.Start();

                process.EnableRaisingEvents = true;
                process.BeginOutputReadLine();
            }
            catch (Exception)
            {
                throw;
            }


            running = true;
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
            {
                return;
            }

            var chars = Enumerable
                .Range(0, char.MaxValue + 1)
                .Select(i => (char)i)
                .Where(c => !char.IsControl(c))
                .ToArray();

            var toKeep = new string(chars);

            var clensed = e.Data.Keep(toKeep);

            output.Add(clensed);
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            running = false;

            Exited(this, new EventArgs());
        }

        /// <summary>
        /// Returns the process output.
        /// </summary>
        /// <returns></returns>
        public IList<string> Output
        {
            get
            {
                return output;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IProcess" /> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if executing; otherwise, <c>false</c>.
        /// </value>
        public bool Running
        {
            get { return running; }
        }

        /// <summary>
        /// Gets the elapsed time since the process was started.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public TimeSpan ElapsedTime
        {
            get { return DateTime.Now - startTime; }

        }

        /// <summary>
        /// Gets the file name of the process.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Kills this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Kill()
        {
            if (running)
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }

                running = false;
            }
        }

        public event EventHandler Exited;
    }
}
