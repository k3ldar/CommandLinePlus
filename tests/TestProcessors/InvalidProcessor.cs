using System.Diagnostics.CodeAnalysis;

namespace CommandLinePlusTests.TestProcessors
{
    [ExcludeFromCodeCoverage]
    internal class InvalidProcessor
    {
        public static void DoNothing()
        {
            // does nothing
        }
    }
}
