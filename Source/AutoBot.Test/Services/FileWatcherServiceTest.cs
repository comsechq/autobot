using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoBot.Core;
using AutoBot.Domain;
using Moq;
using NUnit.Framework;
using Sugar.Configuration;

namespace AutoBot.Services
{
    [TestFixture]
    public class FileWatcherServiceTest : AutoMockingTest
    {
        private FileWatcherService service;

        [SetUp]
        public void SetUp()
        {
            service = Create<FileWatcherService>();
        }

        [Test]
        public void TestInitialize()
        {
            var fileName = Path.GetFullPath("test.txt");

            var watches = new List<ConfigLine> { new ConfigLine { Key = fileName, Value = "test" } };

            Mock<IConfigService>()
                .Setup(call => call.GetValues("watch"))
                .Returns(watches);

            service.Initialize();

            Assert.AreEqual(1, service.FileWatchers.Count);
        }

        [Test]
        public void TestAddWatch()
        {
            var fileName = Path.GetFullPath("test.txt");

            service.AddWatch(fileName, "channel");

            Assert.AreEqual(1, service.FileWatchers.Count);
            Assert.AreEqual(fileName, service.FileWatchers[0].FileName);
            Assert.AreEqual("channel", service.FileWatchers[0].Channel);
        }

        [Test]
        public void TestRemoveWatch()
        {
            var fileNameOne = Path.GetFullPath("test-one.txt");
            var fileNameTwo = Path.GetFullPath("test-two.txt");

            service.FileWatchers.Add(new FileWatcher(fileNameOne, "channel"));
            service.FileWatchers.Add(new FileWatcher(fileNameTwo, "channel"));

            service.RemoveWatch(fileNameOne);

            Assert.AreEqual(1, service.FileWatchers.Count);

            Mock<IConfigService>()
                .Verify(call => call.DeleteValue("watch", fileNameOne));
        }

        [Test]
        public void TestWatchedFileChanged()
        {
            var fileName = Path.GetFullPath("test.txt");
            var e = new FileSystemEventArgs(WatcherChangeTypes.All, "", fileName);
            var watcher = new FileWatcher(fileName, "channel");

            service.FileWatchers.Add(watcher);

            File.WriteAllText(fileName, "test contents");

            service.FileChanged(watcher, e);

            Mock<IChatService>()
                .Verify(call => call.Reply(It.IsAny<Message>(), "test contents"));

            Assert.IsFalse(File.Exists(fileName));
        }

        [Test]
        public void TestWatchedFileChangedFileNotFound()
        {
            var fileName = Path.GetFullPath("test.txt");
            var e = new FileSystemEventArgs(WatcherChangeTypes.All, "", fileName);
            var watcher = new FileWatcher(fileName, "channel");

            service.FileChanged(watcher, e);

            Assert.IsFalse(File.Exists(fileName));
        }
    }
}
