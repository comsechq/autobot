using Moq;
using NUnit.Framework;
using Sugar.Command;
using Sugar.IO;

namespace AutoBot.Services
{
    [TestFixture]
    public class ConfigServiceTest : AutoMockingTest
    {
        private ConfigService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<StubConfigService>();

            Parameters.SetCurrent(string.Empty);
        }

        [Test]
        public void TestGetConfigurationFileName()
        {
            var result = service.GetConfigurationFilename();

            Assert.AreEqual(".\\AutoBot.config", result);
        }

        [Test]
        public void TestGetConfigurationFileNameOverridenViaParameters()
        {
            Parameters.SetCurrent("-config test");

            var result = service.GetConfigurationFilename();

            Assert.AreEqual(".\\test.config", result);
        }

        private void ExpectCallToReadConfiguration()
        {
            const string configuration = @"
[section]
key=test";

            Mock<IFileService>()
                .Setup(call => call.ReadAllText(".\\AutoBot.config"))
                .Returns(configuration);
        }

        [Test]
        public void TestGetConfiguration()
        {
            ExpectCallToReadConfiguration();

            var result = service.GetConfig();

            Assert.AreEqual("test", result.GetValue("section", "key", "default"));

        }

        [Test]
        public void TestGetValue()
        {
            ExpectCallToReadConfiguration();

            var result = service.GetValue("section", "key", "default");

            Assert.AreEqual("test", result);
        }

        [Test]
        public void TestGetValues()
        {
            ExpectCallToReadConfiguration();

            var result = service.GetValues("section");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("key", result[0].Key);
            Assert.AreEqual("test", result[0].Value);
        }
    }
}
