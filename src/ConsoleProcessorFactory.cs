using System.Reflection;

using CommandLinePlus.Abstractions;
using CommandLinePlus.Internal;

namespace CommandLinePlus
{
    internal class ConsoleProcessorFactory : IConsoleProcessorFactory
    {
        public IConsoleProcessor Create(string processName)
        {
            return Create(processName, new CommandLineArgs());
        }

        public IConsoleProcessor Create(string processName, ICommandLineArgs args)
        {
            return Create(processName, args, new ConsoleDisplay(args));
        }

        public IConsoleProcessor Create(string processName, ICommandLineArgs args, IDisplay display)
        {
            return Create(processName, args, display, new[] { Assembly.GetExecutingAssembly() });
        }

        public IConsoleProcessor Create(string processName, ICommandLineArgs args, IDisplay display, Assembly[] assemblies)
        {
            return new ConsoleProcessor(processName, args, display, assemblies);
        }
    }
}
