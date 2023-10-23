using CommandLinePlus.Internal;

namespace CommandLinePlus
{
    /// <summary>
    /// Console processor factory
    /// </summary>
    public class ConsoleProcessorFactory : IConsoleProcessorFactory
    {
        /// <summary>
        /// Create an instance of IConsoleProcessor
        /// </summary>
        /// <param name="processName">Name of process</param>
        /// <param name="processors">Available processors</param>
        /// <param name="args">ICommandLineArgs instance, if null default command line arg processor will be used</param>
        /// <param name="display">IDisplay instance, if null default console display will be used</param>
        /// <returns></returns>
        public IConsoleProcessor Create(string processName, object[] processors, ICommandLineArguments args = null, IDisplay display = null)
        {
            args ??= new CommandLineArguments();

            return new ConsoleProcessorFacade(processName,
                processors,
                args,
                display ?? new ConsoleDisplay(args));
        }
    }
}
