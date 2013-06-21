using System.ComponentModel.Composition;
using System.Linq;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Peeks at the standard output of a running process.
    /// </summary>
    [Export(typeof(IHandler))]
    public class ProcessPeek : Handler<ProcessPeek.Options>
    {
        [Flag("peek")]
        public class Options
        {
            [Parameter("peek", Default = "5")]
            public int Lines { get; set; }
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

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            if (!Process.Running)
            {
                ChatService.Reply(message, "No process running.");

                return;
            }

            var output = Process.Output;

            if (output.Count > options.Lines)
            {
                output = output.Reverse().Take(options.Lines).Reverse().ToList();
            }

            foreach (var line in output)
            {
                ChatService.Reply(message, line);
            }
        }
    }
}
