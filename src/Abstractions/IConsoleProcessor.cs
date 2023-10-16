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
        RunResult Run();
    }
}
