using AutoBot.Domain;
using NUnit.Framework;

namespace AutoBot.Core.MessageParserLinks
{
    [TestFixture]
    public class PingLinkTest
    {
        private PingLink link;

        [SetUp]
        public void SetUp()
        {
            link = new PingLink();
        }

        [Test]
        public void TestCanParseLinkWhenTrue()
        {
            var result = link.CanParse("PING :9787C76B");

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
            var result = link.Parse("PING :9787C76B");

            Assert.AreEqual(MessageType.Ping, result.Type);
            Assert.AreEqual(":9787C76B", result.Body);
        }
    }
}
