
using System;

namespace CommandLinePlus
{
    /// <summary>
    /// Base class for any command line processors
    /// </summary>
    public abstract class BaseCommandLine
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseCommandLine()
        {

        }

        /// <summary>
        /// Constructor used for unit testing purposes only!
        /// </summary>
        /// <param name="args"></param>
        /// <param name="display"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseCommandLine(ICommandLineArguments args, IDisplay display)
            : this()
        {
            CommandLineArgs = args ?? throw new ArgumentNullException(nameof(args));
            Display = display ?? throw new ArgumentNullException(nameof(display));
        }

        /// <summary>
        /// Command line args supplied via factory
        /// </summary>
        protected ICommandLineArguments CommandLineArgs { get; private set; }

        /// <summary>
        /// Display currently being used
        /// </summary>
        protected IDisplay Display { get; private set; }

        /// <summary>
        /// Name of processor, also serves as command line option
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Sort order of processor
        /// </summary>
        public abstract int SortOrder { get; }

        /// <summary>
        /// Indicates wether the processor is enabled or not
        /// </summary>
        public abstract bool IsEnabled { get; }

        /// <summary>
        /// Shows help information for the class
        /// </summary>
        public abstract void DisplayHelp();

        /// <summary>
        /// Default method to be called when no methods match sub option (method name)
        /// </summary>
        public abstract int Execute(string[] args);

        internal void Update(ICommandLineArguments args, IDisplay display)
        {
            CommandLineArgs = args ?? throw new ArgumentNullException(nameof(args));
            Display = display ?? throw new ArgumentNullException(nameof(display));
        }
    }
}
