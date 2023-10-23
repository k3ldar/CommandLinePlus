namespace CommandLinePlus.Internal
{
    internal sealed class DefaultOptions : ICommandLineOptions
    {
        public bool ShowVerbosity => true;

        public bool ShowHelpMessage => true;

        public string SubOptionPrefix => "  ";

        public int SubOptionMinimumLength => 20;

        public string SubOptionSuffix => "  ";

        public string ParameterPrefix => "   ";

        public int ParameterMinimumLength => 18;

        public string ParameterSuffix => "  ";

        public int InternalOptionsMinimumLength => 22;
    }
}
