using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        public class Options
        {
            /// <summary>
            /// Gets or sets the samples.
            /// </summary>
            /// <value>
            /// The samples.
            /// </value>
            [Parameter("cpu", Required = false, Default = "1")]
            public int Samples { get; set; }  
        }

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
            var samples = new List<float>();

            for (var i = 1; i <= options.Samples; i++)
            {
                var cpuUsage = GetCpuUsage(1000);

                samples.Add(cpuUsage);

                ChatService.Reply(message, "CPU Sample {1}: {0:#0.0}%", cpuUsage, i);
            }
     
            if (options.Samples > 1)
            {
                ChatService.Reply(message, "Average CPU: {0:#0.0}% ({1} samples)", samples.Average(), samples.Count);
            }
        }

        private float GetCpuUsage(int sleep)
        {
            var counter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            counter.NextValue();

            Thread.Sleep(sleep);

            return counter.NextValue();
        }
    }
}
