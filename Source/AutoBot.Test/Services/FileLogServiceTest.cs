using System;
using Moq;
using NUnit.Framework;

namespace AutoBot.Services
{
    [TestFixture]
    public class FileLogServiceTest : AutoMockingTest
    {
        private FileLogService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<FileLogService>();
        }

        [Test]
        public void TestGetLogFileName()
        {
            var date = new DateTime(2001, 1, 1);

            Mock<IConfigService>()
                .Setup(call => call.GetValue("logging", "path", "C:\\logs"))
                .Returns("c:\\");

            var result = service.GetLogFileName(date);

            Assert.AreEqual("c:\\2001\\01\\01\\irc-log.txt", result);
        }

        [Test]
        public void TestGetLoggingEnabled()
        {
            Mock<IConfigService>()
                .Setup(call => call.GetValue("logging", "enabled", "false"))
                .Returns("true");

            var result = service.LoggingEnabled();

            Assert.IsTrue(result);

        }
    }
}
