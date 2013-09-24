using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sugar.Configuration;

namespace AutoBot.Services
{
    [TestFixture]
    public class ChannelServiceTest : AutoMockingTest
    {
        private ChannelService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<ChannelService>();
        }

        [Test]
        public void TestJoinChannel()
        {
            Mock<IChatService>()
                .Setup(call => call.Join("test"))
                .Returns(true);

            var result = service.Join("test");

            Assert.IsTrue(result);

            Mock<IConfigService>()
                .Verify(call => call.SetValue("Channels", "test", string.Empty));
        }

        [Test]
        public void TestJoinChannelOnFailure()
        {
            Mock<IChatService>()
                .Setup(call => call.Join("test"))
                .Returns(false);

            var result = service.Join("test");

            Assert.IsFalse(result);

            Mock<IConfigService>()
                .Verify(call => call.SetValue("Channels", "test", string.Empty), Times.Never());
        }

        [Test]
        public void TestLeaveChannel()
        {
            Mock<IChatService>()
                .Setup(call => call.Leave("test"))
                .Returns(true);

            var result = service.Leave("test");

            Assert.IsTrue(result);

            Mock<IConfigService>()
                .Verify(call => call.DeleteValue("Channels", "test"));
        }

        [Test]
        public void TestReconnect()
        {
            var channels = new List<ConfigLine> { new ConfigLine { Key = "test" } };

            Mock<IChatService>()
                .SetupGet(call => call.LoggedIn)
                .Returns(true);

            Mock<IConfigService>()
                .Setup(call => call.GetValues("Channels"))
                .Returns(channels);

            service.Reconnect();

            Mock<IChatService>()
                .Verify(call => call.Join("test"));
        }

        [Test]
        public void TestReconnectWhenNotLoggedIn()
        {
            Mock<IChatService>()
                .SetupGet(call => call.LoggedIn)
                .Returns(false);

            service.Reconnect();

            Mock<IConfigService>()
                .Verify(call => call.GetValues("Channels"), Times.Never());
        }

        [Test]
        public void TestListChannels()
        {
            var lines = new List<ConfigLine>
            {
                new ConfigLine {Key = "one"},
                new ConfigLine {Key = "two"}
            };

            Mock<IConfigService>()
                .Setup(call => call.GetValues("Channels"))
                .Returns(lines);

            var results = service.List();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("one", results[0]);
            Assert.AreEqual("two", results[1]);
        }
    }
}
