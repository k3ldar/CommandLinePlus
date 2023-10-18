using System.Reflection;

namespace CommandLinePlus
{
    /// <summary>
    /// Console processor factory
    /// </summary>
    public interface IConsoleProcessorFactory
    {
        /// <summary>
        /// Creates an instance of IConsoleProcessor any console classes found in executing assembly
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="processors">Classes descending from BaseCommandLine</param>
        /// <param name="args">Command line args instance, default used if null</param>
        /// <param name="display">IDisplay instance, default IDisplay (console) used if null</param>
        /// <returns>IConsoleProcessor instance</returns>
        IConsoleProcessor Create(string processName, object[] processors, ICommandLineArguments args = null, IDisplay display = null);
    }
}
