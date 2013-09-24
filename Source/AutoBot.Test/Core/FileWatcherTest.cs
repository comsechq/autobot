using System.IO;
using System.Threading;
using NUnit.Framework;

namespace AutoBot.Core
{
    [TestFixture]
    public class FileWatcherTest
    {
        private bool changeEventFired;

        [Test]
        public void TestFileWatcher()
        {
            var fileName = Path.GetFullPath("test.txt");

            var watcher = new FileWatcher(fileName, "watcher");

            watcher.Changed += watcher_Changed;

            changeEventFired = false;

            Assert.AreEqual(fileName, watcher.FileName);
            Assert.AreEqual("watcher", watcher.Channel);

            try
            {
                File.WriteAllText(fileName, "test");

                Thread.Sleep(250);

                Assert.IsTrue(changeEventFired);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            changeEventFired = true;
        }
    }
}
