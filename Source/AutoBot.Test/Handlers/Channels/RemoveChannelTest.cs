using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Channels
{
    [TestFixture]
    public class RemoveChannelTest : AutoMockingTest
    {
        private RemoveChannel handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<RemoveChannel>();
        }

        [Test]
        public void TestAddChannel()
        {
            var message = new Message();
            var options = new RemoveChannel.Options { Channel = "test" };

            handler.Receive(message, options);

            Mock<IChannelService>()
                .Verify(call => call.Leave("test"));

            Mock<IChatService>()
                .Verify(call => call.ReplyFormat(message, "Removed channel: {0}", "test"));
        }
    }
}
