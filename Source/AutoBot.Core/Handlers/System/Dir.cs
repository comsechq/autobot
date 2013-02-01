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
    public class Dir : Handler<Dir.Options>
    {
        [Flag("dir")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>
            /// The name of the file.
            /// </value>
            [Parameter("dir", Required = true)]
            public string Path { get; set; }

            [Parameter("pattern", Required = false, Default = "*")]
            public string Pattern { get; set; }

            [Flag("bare")]
            public bool Bare { get; set; }
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
            var path = options.Path;

            if (path.Contains("[DD]")) path = path.Replace("[DD]", DateTime.Now.ToString("dd"));
            if (path.Contains("[MM]")) path = path.Replace("[MM]", DateTime.Now.ToString("MM"));
            if (path.Contains("[YYYY]")) path = path.Replace("[YYYY]", DateTime.Now.ToString("yyyy"));

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, options.Pattern);

                foreach (var file in files)
                {
                    var info = new FileInfo(file);
                    var size = info.Length.ToString("###,###,###,###,###,##0");
                    var date = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss ");

                    if (options.Bare)
                    {
                        var name = date + Path.GetFileName(file) + " (" + size + " bytes)";

                        ChatService.Reply(message, name);
                    }
                    else
                    {
                        ChatService.Reply(message, file);                        
                    }
                }

                if (files.Length == 0)
                {
                    ChatService.Reply(message, "(no files) {0:###,##0} file(s).", files.Length);
                }
            }
            else
            {
                ChatService.Reply(message, "Can't find directory: {0}", path);
            }
        }      
    }
}
