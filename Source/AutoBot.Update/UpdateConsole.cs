using System;
using AutoBot.Services;
using Sugar.Command;

namespace AutoBot
{
    /// <summary>
    /// Main console to update AutoBot.
    /// </summary>
    public class UpdateConsole : BaseConsole
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the update service.
        /// </summary>
        /// <value>
        /// The update service.
        /// </value>
        public IUpdateService UpdateService { get; set; } 
        
        #endregion

        /// <summary>
        /// Main console application logic.
        /// </summary>
        protected override void Main()
        {
            // New Version Available?
            if (UpdateService.IsNewVersionAvailable())
            {
                // Download Update
                var path = UpdateService.DownloadUpdate();

                // Delete existing bot
                UpdateService.RemoveCurrentVersion();

                // Extract Update
                // UpdateService.ExtractUpdate(path);

                // Copy new update in place
                UpdateService.CopyUpdate(path);

                // Clean up
                UpdateService.RemoveUpdate(path);
            }

            // Relaunch bot
            if (Arguments.Contains("-launch"))
            {
                Console.WriteLine("Launching AutoBot...");

                UpdateService.LaunchBot();
            }
        }
    }
}
