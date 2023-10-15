using System.Reflection;

namespace CommandLinePlus.Abstractions
{
    /// <summary>
    /// Console processor factory
    /// </summary>
    public interface IConsoleProcessorFactory
    {
        /// <summary>
        /// Creates an instance of IConsoleProcessor using default args passed into application, standard console 
        /// output and any console classes found in executing assembly
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <returns>IConsoleProcessor instance</returns>
        IConsoleProcessor Create(string processName);

        /// <summary>
        /// Creates an instance of IConsoleProcessor using standard console 
        /// output and any console classes found in executing assembly
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="args">Command line args instance</param>
        /// <returns>IConsoleProcessor instance</returns>
        IConsoleProcessor Create(string processName, ICommandLineArgs args);

        /// <summary>
        /// Creates an instance of IConsoleProcessor any console classes found in executing assembly
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="args">Command line args instance</param>
        /// <param name="display">IDisplay instance</param>
        /// <returns>IConsoleProcessor instance</returns>
        IConsoleProcessor Create(string processName, ICommandLineArgs args, IDisplay display);

        /// <summary>
        /// Creates an instance of IConsoleProcessor any console classes found in executing assembly
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="args">Command line args instance</param>
        /// <param name="display">IDisplay instance</param>
        /// <param name="assemblies">Assemblies containing BaseCommandLine descendants</param>
        /// <returns>IConsoleProcessor instance</returns>
        IConsoleProcessor Create(string processName, ICommandLineArgs args, IDisplay display, Assembly[] assemblies);
    }
}
