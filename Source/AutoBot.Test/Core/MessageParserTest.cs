using AutoBot.Domain;
using NUnit.Framework;

namespace AutoBot.Core
{
    [TestFixture]
    public class MessageParserTest
    {
        private MessageParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new MessageParser();

            parser.Links.Add(new FakeMessageParserLink());
        }

        [Test]
        public void TestParseMessage()
        {
            var result = parser.Parse("CANPARSE");

            Assert.AreEqual("Hello CANPARSE", result.Body);
            Assert.AreEqual(MessageType.PrivateMessage, result.Type);
        }

        [Test]
        public void TestCantParseMessage()
        {
            var result = parser.Parse("CANTPARSE");

            Assert.AreEqual("CANTPARSE", result.Body);
            Assert.AreEqual(MessageType.Unknown, result.Type);
        }
    }
}
