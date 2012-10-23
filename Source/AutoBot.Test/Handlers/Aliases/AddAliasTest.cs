using AutoBot.Domain;
using AutoBot.Services;
using Moq;
using NUnit.Framework;

namespace AutoBot.Handlers.Aliases
{
    [TestFixture]
    public class AddAliasTest : AutoMockingTest
    {
        private AddAlias handler;

        [SetUp]
        public void SetUp()
        {
            handler = Create<AddAlias>();
        }

        [Test]
        public void TestAddAlias()
        {
            var options = new AddAlias.Options();
            options.Name = "test";
            options.Value = "alias";

            handler.Receive(new Message(), options);

            Mock<IAliasService>()
                .Verify(call => call.SetAlias("test", "alias"));
        }
    }
}
