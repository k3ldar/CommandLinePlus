using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;

namespace CommandLinePlusTests.TestProcessors
{
    [ExcludeFromCodeCoverage]
    internal class TestPluginpProcessor : BaseCommandLine
    {
        public override string Name => "Plugin";

        public override int SortOrder => 0;

        public override bool IsEnabled => true;

        public override void DisplayHelp()
        {
            Display.WriteLine("Plugin");
            Display.WriteLine("    Add");
            Display.WriteLine("    Remove");
            Display.WriteLine("    Disable");
            Display.WriteLine("    Enable");
            Display.WriteLine("    -p    Name of plugin");
        }

        public override int Execute(string[] args)
        {
            // default handler 
            MethodsCalled.Add(nameof(Execute));
            ArgsPassedToExecute = args;
            return 0;
        }

        public int Add(string p)
        {
            MethodsCalled.Add(nameof(Add));
            ArgsPassed.Add(p);
            return 0;
        }

        public int Remove(string p)
        {
            MethodsCalled.Add(nameof(Remove));
            ArgsPassed.Add(p);
            return 0;
        }

        public int Disable(string p)
        {
            MethodsCalled.Add(nameof(Disable));
            ArgsPassed.Add(p);
            return 0;
        }

        public int Enable(string p)
        {
            MethodsCalled.Add(nameof(Enable));
            ArgsPassed.Add(p);
            return 0;
        }

        public string[] ArgsPassedToExecute { get; private set; }

        public List<string> MethodsCalled { get; private set; } = new();

        public List<string> ArgsPassed { get; private set; } = new();
    }
}
