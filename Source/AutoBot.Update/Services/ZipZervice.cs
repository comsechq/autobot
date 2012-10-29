using System;
using System.IO;
using SevenZipLib;

namespace AutoBot.Services
{
    public class ZipZervice : IZipService
    {
        /// <summary>
        /// Decompresses the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void DecompressFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new ApplicationException(string.Format("Can't locate: {0}", filename));
            }

            Console.WriteLine("Decompressing: {0}", filename);

            var path = Path.GetDirectoryName(filename) ?? string.Empty;

            using (var archive = new SevenZipArchive(filename))
            {
                foreach (var entry in archive)
                {
                    if (entry.IsDirectory) continue;

                    var file = entry.FileName;

                    Console.WriteLine("Extracting File: {0}", file);

                    var filePath = Path.Combine(path, file);
                    var fileDirectory = Path.GetDirectoryName(filePath) ?? Environment.CurrentDirectory;

                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }

                    using (var stream = File.Create(filePath))
                    {
                        entry.Extract(stream, ExtractOptions.OverwriteExistingFiles);

                        stream.Flush();

                        stream.Close();
                    }
                }
            }
        }
    }
}
