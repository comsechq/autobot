using AutoBot.Core;
using Sugar.Command;

namespace AutoBot
{
    /// <summary>
    /// Update program to run automatic Bot updates
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            // Register types
            Stencil.Defaults.Assemblies.Add(typeof(Program).Assembly);
            Stencil.Defaults.Assemblies.Add(typeof(Parameters).Assembly);
            Stencil.Instance.Initilize();

            // Initilize console
            var console = Stencil.Instance.Resolve<UpdateConsole>();
            console.Run(args);
        }
    }
}
