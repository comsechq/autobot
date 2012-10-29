namespace AutoBot.Services
{
    /// <summary>
    /// Service to manipulate ZIP files
    /// </summary>
    public interface IZipService
    {
        /// <summary>
        /// Decompresses the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void DecompressFile(string filename);
    }
}
