using AutoBot.Domain;
using Moq;
using NUnit.Framework;
using Sugar.Command;
using Sugar.Configuration;

namespace AutoBot.Services
{
    [TestFixture]
    public class CredentialServiceTest : AutoMockingTest
    {
        private CredentialService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<CredentialService>();
        }

        [Test]
        public void TestGetCredentials()
        {
            var config = new Config();
            config.SetValue("Credentials", "Server", "irc.example.com");
            config.SetValue("Credentials", "Port", "123");
            config.SetValue("Credentials", "Password", "Password");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var result = service.GetCredentials();

            Assert.AreEqual("irc.example.com", result.Server);
            Assert.AreEqual(123, result.Port);
            Assert.AreEqual("Password", result.Password);
        }

        [Test]
        public void TestStoreCredentials()
        {
            var config = new Config();

            var credentials = new Credentials
            {
                Server = "irc.example.com",
                Port = 123,
                Password = "Password"
            };

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            service.SetCredentials(credentials);

            Assert.AreEqual("123", config.GetValue("Credentials", "Port", string.Empty));
            Assert.AreEqual("irc.example.com", config.GetValue("Credentials", "Server", string.Empty));
            Assert.AreEqual("Password", config.GetValue("Credentials", "Password", string.Empty));
        }

        [Test]
        public void TestGetCredentialsWhenSet()
        {
            var config = new Config();
            config.SetValue("Credentials", "Server", "example.com");

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var result = service.CredentialsSet();

            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestGetCredentialsWhenNotSet()
        {
            var config = new Config();

            Mock<IConfigService>()
                .Setup(call => call.GetConfig())
                .Returns(config);

            var result = service.CredentialsSet();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void TestGetCredentialsFromParameters()
        {
            var parameters = new Parameters(@"-s server -p 101 -pwd password -n nick -r ""real name""");

            var result = service.ParseCredentials(parameters);

            Assert.AreEqual(result.Server, "server");
            Assert.AreEqual(result.Port, 101);
            Assert.AreEqual(result.Password, "password");
            Assert.AreEqual(result.Nick, "nick");
            Assert.AreEqual(result.Name, "real name");
        }

        [Test]
        public void TestValidateCredentials()
        {
            var credentials = new Credentials
            {
                Server = "example.com",
                Port = 6667,
                Password = "password",
                Nick = "nick",
                Name = "name"
            };

            var result = service.Validate(credentials);

            Assert.IsTrue(result);
        }
    }
}
