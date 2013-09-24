using AutoBot.Domain;
using NUnit.Framework;

namespace AutoBot.Core.MessageParserLinks
{
    [TestFixture]
    public class SetModeLinkTest
    {
        private SetModeLink link;

        [SetUp]
        public void SetUp()
        {
            link = new SetModeLink();
        }

        [Test]
        public void TestCanParseLinkWhenTrue()
        {
            Context.Nick = "test";

            var result = link.CanParse(":test MODE test :+iwz");

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
            var result = link.Parse(":sandwich MODE sandwich :+iwz");

            Assert.AreEqual(MessageType.SetMode, result.Type);
            Assert.AreEqual("iwz", result.Body);
        }
    }
}
