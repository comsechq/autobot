using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.System
{
    [TestFixture]
    public class HelpTest : AutoMockingTest
    {
        private Help handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<Help>();
        }

        [Test]
        public void TestGetHelpText()
        {
            var result = handler.GetHelpTextFromAssembly();

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [Test]
        public void TestGetHelpSection()
        {
            const string helpText = @"
Help Text
Example
:section
section text
here";
            var result = handler.GetHelpTopic(helpText, "section");

            Assert.AreEqual("section text\r\nhere\r\n", result);
        }

        [Test]
        public void TestGetHelpSectionTerminatesBeforeNextSection()
        {
            const string helpText = @"
Help Text
Example
:section
section text
here
:second section
here";
            var result = handler.GetHelpTopic(helpText, "section");

            Assert.AreEqual("section text\r\nhere\r\n", result);
        }

        [Test]
        public void TestGetHelpSectionGetDefaultSection()
        {
            const string helpText = @"Help Text
Example
:section
section text
here
:second section
here";
            var result = handler.GetHelpTopic(helpText, "");

            Assert.AreEqual("Help Text\r\nExample\r\n", result);
        }

        [Test]
        public void TestGetHelp()
        {
            var message = new Message();
            message.Body = "help hello";

            handler.Receive(message);

            Mock<IChatService>()
                .Verify(call => call.Reply(message, "Says hello"));
        }
    }
}
