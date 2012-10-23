using System.ComponentModel.Composition;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Displays the current processes running
    /// </summary>
    [Export(typeof(IHandler))]
    public class Recycle : Handler<Recycle.Options>
    {
        [Flag("recycle")]
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
        /// Gets or sets the update service.
        /// </summary>
        /// <value>
        /// The update service.
        /// </value>
        public IUpdateService UpdateService { get; set; }

        #endregion

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Options options)
        {
            ChatService.Reply(message, "Recycling bot.");

            UpdateService.RunLatestVersion(false, true);
        }
    }
}
