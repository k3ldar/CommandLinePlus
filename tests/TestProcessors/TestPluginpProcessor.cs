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

        public override void DisplayHelp(IDisplay display)
        {
            display.WriteLine("Plugin");
            display.WriteLine("    Add");
            display.WriteLine("    Remove");
            display.WriteLine("    Disable");
            display.WriteLine("    Enable");
            display.WriteLine("    -p    Name of plugin");
        }

        public override void Execute(string[] args)
        {
            // default handler 
            MethodsCalled.Add(nameof(Execute));
            ArgsPassedToExecute = args;
        }

        public void Add(string p)
        {
            MethodsCalled.Add(nameof(Add));
            ArgsPassed.Add(p);
        }

        public void Remove(string p)
        {
            MethodsCalled.Add(nameof(Remove));
            ArgsPassed.Add(p);
        }

        public void Disable(string p)
        {
            MethodsCalled.Add(nameof(Disable));
            ArgsPassed.Add(p);
        }

        public void Enable(string p)
        {
            MethodsCalled.Add(nameof(Enable));
            ArgsPassed.Add(p);
        }

        public string[] ArgsPassedToExecute { get; private set; }

        public List<string> MethodsCalled { get; private set; } = new();

        public List<string> ArgsPassed { get; private set; } = new();
    }
}
