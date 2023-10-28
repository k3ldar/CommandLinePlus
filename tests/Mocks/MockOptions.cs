using CommandLinePlus;

namespace CommandLinePlusTests.Mocks
{
    internal class MockOptions : ICommandLineOptions
    {
        public bool ShowVerbosity { get; set; }

        public bool ShowHelpMessage { get; set; }

        public string SubOptionPrefix { get; set; }

        public int SubOptionMinimumLength { get; set; }

        public string SubOptionSuffix { get; set; }

        public string ParameterPrefix { get; set; }

        public int ParameterMinimumLength { get; set; }

        public string ParameterSuffix { get; set; }

        public int InternalOptionsMinimumLength { get; set; }

        public bool CaseSensitiveOptionNames { get; set; }

        public bool CaseSensitiveSubOptionNames { get; set; }

        public bool CaseSensitiveParameterNames { get; set; }
    }
}
