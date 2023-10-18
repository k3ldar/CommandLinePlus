
namespace CommandLinePlus
{
    /// <summary>
    /// Base class for any command line processors
    /// </summary>
    public abstract class BaseCommandLine
    {
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
        public abstract void DisplayHelp(IDisplay display);

        /// <summary>
        /// Default method to be called when no methods match sub option (method name)
        /// </summary>
        /// <param name="args">all args passed to application, minus primary and sub names</param>
        public abstract void Execute(string[] args);
    }
}
