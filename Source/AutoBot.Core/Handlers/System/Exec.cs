using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays the current CPU usage
    /// </summary>
    [Export(typeof(IHandler))]
    public class Exec : Handler<Exec.Options>
    {
        public class Options
        {
            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>
            /// The name of the file.
            /// </value>
            [Parameter("exec", Required = true)]
            public string FileName { get; set; }

            /// <summary>
            /// Gets or sets the arguments.
            /// </summary>
            /// <value>
            /// The arguments.
            /// </value>
            [Parameter("args")]
            public string Arguments { get; set; }

            /// <summary>
            /// Gets or sets the status.
            /// </summary>
            /// <value>
            /// The status.
            /// </value>
            [Parameter("status")]
            public string Status { get; set; }
        }

        private readonly IList<string> lines = new List<string>();
        private bool executing;

        #region Dependencies
        
        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            if (executing)
            {
                ChatService.Reply(message, "Can't you see, I'm busy!");

                return;                
            }

            var start = DateTime.Now;

            ChatService.Reply(message, "Starting: {0}", options.FileName);
            ChatService.SetStatus(Status.Busy, options.Status);

            lines.Clear();

            var process = new Process
            {              
                StartInfo =
                {
                    UseShellExecute = false, 
                    RedirectStandardOutput = true,
                    FileName = options.FileName,
                    Arguments = options.Arguments
                }
            };

            process.OutputDataReceived += process_OutputDataReceived;

            Console.WriteLine("Starting Process");

            try
            {
                executing = true;

                Console.WriteLine("Starting Process");

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                Console.WriteLine("Finished Process");

                var time = DateTime.Now - start;
                ChatService.Reply(message, "Finished process in {0}:{1:00} minutes.", time.Minutes, time.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ChatService.Reply(message, "Error: " + ex.Message);
            }
            finally
            {
                executing = false;
                ChatService.SetStatus(Status.Available, string.Empty);                
            }

            foreach (var line in lines)
            {
                ChatService.Reply(message, line);
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                var chars = Enumerable
                    .Range(0, char.MaxValue + 1)
                    .Select(i => (char)i)
                    .Where(c => !char.IsControl(c))
                    .ToArray();

                var toKeep = new string(chars);

                var line = e.Data.Keep(toKeep).TrimTo(80, string.Empty);

                lines.Add(line);

                if (lines.Count > 10)
                {
                    lines.RemoveAt(0);
                }
            }
        }
    }
}
