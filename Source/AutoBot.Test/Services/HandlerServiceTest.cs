using AutoBot.Core;
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
        public void TestHandleMessageWhenDirectMessage()
        {
            var message = new Message { To = "nick", Body = "three", Type = MessageType.PrivateMessage };

            Context.Nick = "nick";

            service.Handle(message);

            Mock<INicknameService>()
                .Verify(call => call.IsAddressedToMe(message), Times.Never());

            Assert.IsFalse(((FakeHandler)service.Handlers[0]).Fired);
            Assert.IsFalse(((FakeHandler)service.Handlers[1]).Fired);
            Assert.IsTrue(((FakeHandler)service.Handlers[2]).Fired);
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
                .Setup(call => call.FormatAlias("two", "two"))
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

        [Test]
        public void TestHandleMessageWhenLoggingEnabled()
        {
            var message = new Message { Body = "test", Type = MessageType.PrivateMessage };

            Mock<ILogService>()
                .Setup(call => call.LoggingEnabled())
                .Returns(true);

            Mock<INicknameService>()
                .Setup(call => call.IsAddressedToMe(message))
                .Returns(true);

            service.Handle(message);

            Mock<ILogService>()
                .Verify(call => call.Log(message));
        }
    }
}
