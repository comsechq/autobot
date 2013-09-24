using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Channels
{
    [TestFixture]
    public class AddChannelTest : AutoMockingTest
    {
        private AddChannel handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<AddChannel>();
        }

        [Test]
        public void TestAddChannel()
        {
            var message = new Message();
            var options = new AddChannel.Options { Channel = "test" };

            handler.Receive(message, options);

            Mock<IChannelService>()
                .Verify(call => call.Join("test"));

            Mock<IChatService>()
                .Verify(call => call.ReplyFormat(message, "Added channel: {0}", "test"));
        }
    }
}
