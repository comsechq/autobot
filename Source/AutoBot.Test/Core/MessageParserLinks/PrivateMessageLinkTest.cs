using AutoBot.Domain;
using NUnit.Framework;

namespace AutoBot.Core.MessageParserLinks
{
    [TestFixture]
    public class PrivateMessageLinkTest
    {
        private PrivateMessageLink link;

        [SetUp]
        public void SetUp()
        {
            link = new PrivateMessageLink();
        }

        [Test]
        public void TestCanParseLinkWhenTrue()
        {
            var result = link.CanParse(":dave!host@example.com PRIVMSG bob :hello world");

            Assert.IsTrue(result);
        }

        [Test]
        public void TestCanParseLinkWhenFalse()
        {
            var result = link.CanParse("NICK test");

            Assert.IsFalse(result);
        }

        [Test]
        public void TestParseLink()
        {
            var result = link.Parse(":dave!host@example.com PRIVMSG bob :hello world");

            Assert.AreEqual(MessageType.PrivateMessage, result.Type);
            Assert.AreEqual("hello world", result.Body);
            Assert.AreEqual("dave", result.From);
            Assert.AreEqual("host@example.com", result.UserHost);
            Assert.AreEqual("bob", result.To);
        }


        [Test]
        public void TestParseLinkWithColon()
        {
            var result = link.Parse(":dave!host@example.com PRIVMSG bob :hello world: this is the end");

            Assert.AreEqual(MessageType.PrivateMessage, result.Type);
            Assert.AreEqual("hello world: this is the end", result.Body);
            Assert.AreEqual("dave", result.From);
            Assert.AreEqual("host@example.com", result.UserHost);
            Assert.AreEqual("bob", result.To);
        }
    }
}
