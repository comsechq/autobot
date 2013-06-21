using System.ComponentModel.Composition;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Kills a started process
    /// </summary>
    [Export(typeof(IHandler))]
    public class ProcessKill : Handler<ProcessKill.Options>
    {
        [Flag("kill")]
        public class Options {}

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

            Process.Kill();

            ChatService.ReplyFormat(message, "Process '{0}' killed.", Process.FileName);
        }
    }
}
