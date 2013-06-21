using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays the current CPU usage
    /// </summary>
    [Export(typeof(IHandler))]
    public class ProcessStart : Handler<ProcessStart.Options>
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

            [Flag("echo")]
            public bool EchoOutput { get; set; }
        }

        #region Dependencies

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>
        /// The process.
        /// </value>
        public IProcess Process { get; set; }

        #endregion

        // Channel to send process notifications to
        private string channel;

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            if (Process.Running)
            {
                ChatService.Reply(message, "Already running a process.");
                return;
            }

            if (!File.Exists(options.FileName))
            {
                ChatService.ReplyFormat(message, "Not found: {0}", options.FileName);
                return;
            }

            ChatService.ReplyFormat(message, "Starting: {0} {1}", options.FileName, options.Arguments);

            Process.Exited += Process_Exited;

            Process.Start(options.FileName, options.Arguments);

            // Set process finish message channel
            channel = message.To.StartsWith("#") ? message.To : message.From;
        }

        /// <summary>
        /// Handles the Exited event of the Process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void Process_Exited(object sender, EventArgs e)
        {
            var output = Process.Output;

            if (output.Count > 10)
            {
                output = output.Reverse().Take(10).Reverse().ToList();
            }

            foreach (var line in output)
            {
                ChatService.Reply(channel, line);
            }

            ChatService.ReplyFormat(channel, "Finished process in {0}:{1:00} mins.", Process.ElapsedTime.TotalMinutes, Process.ElapsedTime.Seconds);
        }
    }
}
