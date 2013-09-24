using System.Collections.Generic;
using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Channels
{
    [TestFixture]
    public class ListChannelsTest : AutoMockingTest
    {
        private ListChannels handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<ListChannels>();
        }

        [Test]
        public void TestListChannel()
        {
            var message = new Message();
            var options = new ListChannels.Options();

            Mock<IChannelService>()
                .Setup(call => call.List())
                .Returns(new List<string> { "one", "two", "three" });

            handler.Receive(message, options);

            Mock<IChatService>()
                .Verify(call => call.ReplyFormat(message, "one"));

            Mock<IChatService>()
                .Verify(call => call.ReplyFormat(message, "two"));
            
            Mock<IChatService>()
                .Verify(call => call.ReplyFormat(message, "three"));
        }
    }
}
