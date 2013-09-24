using System.Collections.Generic;
using System.IO;
using AutoBot.Core;
using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Watches
{
    [TestFixture]
    public class ListWatchTest : AutoMockingTest
    {
        private ListWatches handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<ListWatches>();
        }

        [Test]
        public void TestAddWatch()
        {
            var message = new Message {Body = "watch list"};
            var fileName = Path.GetFullPath("test.txt");

            var watchers = new List<IFileWatcher> { new FileWatcher(fileName, "channel") };

            Mock<IFileWatcherService>()
                .SetupGet(call => call.FileWatchers)
                .Returns(watchers);

            handler.Receive(message);

            Mock<IChatService>()
                .Verify(call => call.Reply(message, fileName));
        }
    }
}
