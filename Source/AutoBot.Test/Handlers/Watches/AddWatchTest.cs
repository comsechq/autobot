using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Watches
{
    [TestFixture]
    public class AddWatchTest : AutoMockingTest
    {
        private AddWatch handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<AddWatch>();
        }

        [Test]
        public void TestAddWatch()
        {
            var message = new Message { Body = "watch c:\\test.txt channel #test" };

            handler.Receive(message);

            Mock<IFileWatcherService>()
                .Verify(call => call.AddWatch("c:\\test.txt", "#test"));
        }
    }
}
