using Moq;
using NUnit.Framework;

namespace AutoBot.Services
{
    [TestFixture]
    public class ChannelServiceTest : AutoMockingTest
    {
        private ChannelService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<ChannelService>();
        }

    
    }
}
