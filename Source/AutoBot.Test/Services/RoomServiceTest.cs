using System;
using System.IO;
using AutoBot.Domain;
using Moq;
using NUnit.Framework;
using Sugar.Net;

namespace AutoBot.Services
{
    [TestFixture]
    public class RoomServiceTest : AutoMockingTest
    {
        private ChannelService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<ChannelService>();
        }

    
    }
}
