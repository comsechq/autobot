using System.ComponentModel.Composition;
using System.Diagnostics;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays the current CPU usage
    /// </summary>
    [Export(typeof(IHandler))]
    public class Cpu : Handler<Cpu.Options>
    {
        [Flag("cpu")]
        public class Options {}

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
            var counter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            ChatService.Reply(message, "CPU Usage: " + counter.NextValue() + "%");
        }
    }
}
