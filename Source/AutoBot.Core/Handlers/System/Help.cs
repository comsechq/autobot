using System;
using System.IO;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays the current CPU usage
    /// </summary>
    public class Help : Handler<Help.Options>
    {
        [Flag("help")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the samples.
            /// </summary>
            /// <value>
            /// The samples.
            /// </value>
            [Parameter("help", Required = false, Default = "")]
            public string Topic { get; set; }
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
            var result = string.Empty;
            var assembly = GetType().Assembly;

            using (var stream = assembly.GetManifestResourceStream("AutoBot.Help.txt"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            var lines = result.Split(Environment.NewLine);

            foreach (var line in lines)
            {
                ChatService.Reply(message, line);
            }
        }
    }
}
