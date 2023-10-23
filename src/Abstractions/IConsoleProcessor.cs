using System;

namespace CommandLinePlus
{
    /// <summary>
    /// Command line processor
    /// </summary>
    public interface IConsoleProcessor
    {
        /// <summary>
        /// Runs the processor
        /// </summary>
        /// <returns>RunResult</returns>
        RunResult Run(out int result);

        /// <summary>
        /// Runs the processor with custom options
        /// </summary>
        /// <param name="options"></param>
        /// <param name="result"></param>
        /// <returns>RunResult</returns>
        RunResult Run(ICommandLineOptions options, out int result);

        /// <summary>
        /// Default run processor without result code
        /// </summary>
        /// <returns>RunResult</returns>
        [Obsolete("Deprecated and will be removed in future", true)]
        RunResult Run();
    }
}
