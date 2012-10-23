using AutoBot.Domain;
using AutoBot.Handlers;
using Moq;
using NUnit.Framework;

namespace AutoBot.Services
{
    [TestFixture]
    public class HandlerServiceTest : AutoMockingTest
    {
        private HandlerService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<HandlerService>();

            service.Handlers.Add(new FakeHandler("one"));
            service.Handlers.Add(new FakeHandler("two"));
            service.Handlers.Add(new FakeHandler("three"));
        }

        [Test]
        public void TestHandleMessage()
        {
            var message = new Message { Body = "two", Type = MessageType.PrivateMessage };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            service.Handle(message);

            Assert.IsFalse(((FakeHandler) service.Handlers[0]).Fired);
            Assert.IsTrue(((FakeHandler) service.Handlers[1]).Fired);
            Assert.IsFalse(((FakeHandler) service.Handlers[2]).Fired);
        }

        [Test]
        public void TestHandleMessageWhenNotAddressedToBot()
        {
            var message = new Message { Body = "four", Type = MessageType.PrivateMessage };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(false);

            service.Handle(message);

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[2]).Fired);
        }

        [Test]
        public void TestHandleMessageWhenAliased()
        {
            var message = new Message { Body = "two", Type = MessageType.PrivateMessage };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            Mock<IAliasService>()
                .Setup(call => call.IsAlias("two"))
                .Returns(true);

            Mock<IAliasService>()
                .Setup(call => call.GetAlias("two"))
                .Returns("three");

            service.Handle(message);

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsTrue(((FakeHandler)service.Handlers[2]).Fired);
        }

        [Test]
        public void TestHandleMessageWhenAliasedButBypassed()
        {
            var message = new Message { Body = "!two", Type = MessageType.PrivateMessage };

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            Mock<IAliasService>()
                .Setup(call => call.IsAlias("!two"))
                .Returns(false);

            service.Handle(message);

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsTrue(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[2]).Fired);
        }      
    }
}
