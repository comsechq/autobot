using System;
using System.ComponentModel.Composition;
using System.IO;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays a directory output
    /// </summary>
    [Export(typeof(IHandler))]
    public class Tail : Handler<Tail.Options>
    {
        [Flag("tail")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>
            /// The name of the file.
            /// </value>
            [Parameter("tail", Required = true)]
            public string Filename { get; set; }

            [Parameter("lines", Required = false, Default = "10")]
            public int Lines { get; set; }

            [Parameter("pattern", Required = false)]
            public string Pattern { get; set; }
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
        /// Gets or sets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        public IConsole Console { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            var filename = options.Filename;

            if (filename.Contains("[DD]")) filename = filename.Replace("[DD]", DateTime.Now.ToString("dd"));
            if (filename.Contains("[MM]")) filename = filename.Replace("[MM]", DateTime.Now.ToString("MM"));
            if (filename.Contains("[YYYY]")) filename = filename.Replace("[YYYY]", DateTime.Now.ToString("yyyy"));

            if (Directory.Exists(filename))
            {
                var files = Directory.GetFiles(filename, "*");

                foreach (var file in files)
                {
                    if (file.Contains(options.Pattern))
                    {
                        TailFile(message, options, file);
                    } 
                }
            }
            else if (File.Exists(filename))
            {
                TailFile(message, options, filename);
            }
            else
            {
                ChatService.ReplyFormat(message, "Can't find file: {0}", filename);
            }
        }

        private void TailFile(Message message, Options options, string filename)
        {
            var lines = File.ReadAllLines(filename);
            var start = lines.Length - options.Lines;
            if (start < 0) start = 0;

            for (var i = 0; i <= options.Lines; i++)
            {
                if (start + i >= lines.Length) break;

                ChatService.Reply(message, lines[start + i]);
            }
        }
    }
}
