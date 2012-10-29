using System.ComponentModel.Composition;
using System.IO;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.Update
{
    /// <summary>
    /// Updates the current bot
    /// </summary>
    [Export(typeof(IHandler))]
    public class UpdateNow : Handler<UpdateNow.Options>
    {
        [Flag("update", "now")]
        public class Options {}

        /// <summary>
        /// Gets or sets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public IChatService ChatService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            var updateFileName = Path.Combine(Parameters.Directory, "Update", "update.exe");


        }
    }
}
