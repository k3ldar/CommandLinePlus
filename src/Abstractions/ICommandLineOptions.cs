namespace CommandLinePlus
{
    /// <summary>
    /// Options for configuring command line plus
    /// </summary>
    public interface ICommandLineOptions
    {
        /// <summary>
        /// Show verbosity for help
        /// </summary>
        bool ShowVerbosity { get; }

        /// <summary>
        /// Shows help message
        /// </summary>
        bool ShowHelpMessage { get; }

        /// <summary>
        /// Prefix for sub options (\t or spaces etc)
        /// </summary>
        string SubOptionPrefix { get; }

        /// <summary>
        /// Length of text to display sub options with, for alignment purposes
        /// </summary>
        int SubOptionMinimumLength { get; }

        /// <summary>
        /// Suffix displayed after sub option name
        /// </summary>
        string SubOptionSuffix { get; }

        /// <summary>
        /// Prefix for each parameter
        /// </summary>
        string ParameterPrefix { get; }

        /// <summary>
        /// Minimum length for each parameter
        /// </summary>
        int ParameterMinimumLength { get; }

        /// <summary>
        /// Suffix for each parameter
        /// </summary>
        string ParameterSuffix { get; }

        /// <summary>
        /// Minimum length for internal options
        /// </summary>
        int InternalOptionsMinimumLength { get; }

        /// <summary>
        /// Option names as typed in are case sensitive or not
        /// </summary>
        bool CaseSensitiveOptionNames { get; }

        /// <summary>
        /// Sub option names as typed in are case sensitive or not
        /// </summary>
        bool CaseSensitiveSubOptionNames { get; }

        /// <summary>
        /// Parameter names as typed in are case sensitive or not
        /// </summary>
        bool CaseSensitiveParameterNames { get; }
    }
}
