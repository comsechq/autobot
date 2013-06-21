using System.IO;
using AutoBot.Domain;
using AutoBot.Services;
using Sugar;
using Sugar.Command;

namespace AutoBot.Handlers.System
{
    /// <summary>
    /// Says hello to the sender
    /// </summary>
    public class Version : Handler<Version.Options>
    {
        [Flag("version")]
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
            var assembly = typeof (Version).Assembly.GetName().Version;
            var location = typeof (Version).Assembly.Location;
            var built = File.GetLastWriteTime(location);

            ChatService.ReplyFormat(message, "Version: {0} - Built: {1:dd MMM yyyy} at {1:HH:mm} ({2})", assembly, built, built.ToHumanReadableString());
        }
    }
}
