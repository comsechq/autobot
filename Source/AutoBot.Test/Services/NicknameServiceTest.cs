using AutoBot.Domain;
using Moq;
using NUnit.Framework;
using Sugar.Configuration;

namespace AutoBot.Services
{
    [TestFixture]
    public class NicknameServiceTest : AutoMockingTest
    {
        private NicknameService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<NicknameService>();
        }

        [Test]
        public void TestAddNickname()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.Add("bob");

            var section = config.GetSection("Nicknames");

            Assert.AreEqual(1, section.Count);
            Assert.AreEqual("bob", section[0].Key);
        }

        [Test]
        public void TestRemoveNickname()
        {
            var config = new Config();
            config.SetValue("Nicknames", "bob", string.Empty);
            config.SetValue("Nicknames", "alice", string.Empty);

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.Remove("bob");

            var section = config.GetSection("Nicknames");

            Assert.AreEqual(1, section.Count);
            Assert.AreEqual("alice", section[0].Key);
        }

        private void ExpectCallToGetConfiguration()
        {
            var config = new Config();
            config.SetValue("Nicknames", "bob", string.Empty);
            config.SetValue("Nicknames", "alice", string.Empty);

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);
        }

        [Test]
        public void TestListNicknames()
        {
            ExpectCallToGetConfiguration();

            var results = service.List();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("bob", results[0]);
            Assert.AreEqual("alice", results[1]);
        }

        [Test]
        public void TestMessageIsAddressedToMe()
        {
            ExpectCallToGetConfiguration();

            var message = new Message();
            message.Body = "bob hello";

            var result = service.IsAddressedToMe(message);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestMessageIsNotAddressedToMe()
        {
            ExpectCallToGetConfiguration();

            var message = new Message();
            message.Body = "charlie hello";

            var result = service.IsAddressedToMe(message);

            Assert.IsFalse(result);
        }

        [Test]
        public void TestMessageIsAddressedToMeIgnoresBlankMessages()
        {
            var message = new Message();
            message.Body = "";

            var result = service.IsAddressedToMe(message);

            Assert.IsFalse(result);

            Mock<IConfigService>()
                .Verify(call => call.GetConfig(), Times.Never());
        }
    }
}
