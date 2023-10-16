using System;
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
            ArgsPassedToExecute = args;
        }

        public void Add(string p)
        {
            // test method
        }

        public void Remove(string p)
        {
            // test method
        }

        public void Disable(string p)
        {
            // test method
        }

        public void Enabled(string p)
        {
            // test method
        }

        public string[] ArgsPassedToExecute { get; private set; }
    }
}
