using System;
using System.IO;
using System.Text;
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
            var helpText = GetHelpTextFromAssembly();
            var section = GetHelpTopic(helpText, options.Topic);

            var lines = section.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                ChatService.Reply(message, line);
            }
        }

        /// <summary>
        /// Gets the help text from assembly.
        /// </summary>
        /// <returns></returns>
        public string GetHelpTextFromAssembly()
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

            return result;
        }

        /// <summary>
        /// Gets a help section from the main help text.
        /// </summary>
        /// <param name="helpText">The help text.</param>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        public string GetHelpTopic(string helpText, string section)
        {
            var result = new StringBuilder();

            var lines = helpText.Split(Environment.NewLine);

            var inSection = false;

            if (string.IsNullOrEmpty(section))
            {
                inSection = true;
            }

            foreach (var line in lines)
            {
                if (line.StartsWith(":"))
                {
                    if (inSection)
                    {
                        inSection = false;
                    }
                    else
                    {
                        var match = string.Compare(line, ":" + section, StringComparison.InvariantCultureIgnoreCase) == 0;

                        if (match)
                        {
                            inSection = true;
                        }
                    }                    
                }
                else if (inSection)
                {
                    result.AppendLine(line);
                }
            }


            return result.ToString();
        }
    }
}
