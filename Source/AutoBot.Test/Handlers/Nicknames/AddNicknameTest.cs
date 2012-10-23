using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Nicknames
{
    [TestFixture]
    public class AddNicknameTest : AutoMockingTest
    {
        private AddNickname handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<AddNickname>();
        }

        [Test]
        public void TestAddNickname()
        {
            var message = new Message();

            message.Body = "nick add bob";

            handler.Receive(message);

            Mock<INicknameService>()
                .Verify(call => call.Add("bob"));
        }
    }
}
