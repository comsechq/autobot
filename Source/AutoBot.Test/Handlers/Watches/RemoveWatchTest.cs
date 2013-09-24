using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Watches
{
    [TestFixture]
    public class RemoveWatchTest : AutoMockingTest
    {
        private RemoveWatch handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<RemoveWatch>();
        }

        [Test]
        public void TestAddWatch()
        {
            var message = new Message {Body = "watch remove test.txt"};
            
            handler.Receive(message);

            Mock<IFileWatcherService>()
                .Verify(call => call.RemoveWatch("test.txt"));
        }
    }
}
