using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Nicknames
{
    [TestFixture]
    public class RemoveNicknameTest : AutoMockingTest
    {
        private RemoveNickname handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<RemoveNickname>();
        }

        [Test]
        public void TestAddNickname()
        {
            var message = new Message();

            message.Body = "nick remove tracy";

            handler.Receive(message);

            Mock<INicknameService>()
                .Verify(call => call.Remove("tracy"));
        }
    }
}
