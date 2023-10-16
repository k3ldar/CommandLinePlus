namespace CommandLinePlus
{
    /// <summary>
    /// Result of running the operation
    /// </summary>
    public enum RunResult
    {
        /// <summary>
        /// Nothing, nada nout
        /// </summary>
        None = 0,

        /// <summary>
        /// Help was displayed
        /// </summary>
        DisplayHelp = 1,

        /// <summary>
        /// Not enough primary candidates were found to process
        /// </summary>
        NotEnoughCandidates = 2,

        /// <summary>
        /// Too many primary Candidates found
        /// </summary>
        TooManyCandidates = 3,

        /// <summary>
        /// Sub option not found, default sub option used instead
        /// </summary>
        DefaultSubOptionUsed = 4,
    }

    /// <summary>
    /// Verbosity level for output of data
    /// </summary>
    public enum VerbosityLevel
    {
        /// <summary>
        /// Minimal information is displayed
        /// </summary>
        Quiet = 0,

        /// <summary>
        /// Default information is displayed
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Diagnostic information is displayed
        /// </summary>
        Diagnostic = 2,

        /// <summary>
        /// All information is displayed
        /// </summary>
        Full = 3,
    }
}
