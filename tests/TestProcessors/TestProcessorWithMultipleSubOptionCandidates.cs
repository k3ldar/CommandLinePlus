using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLinePlus;

namespace CommandLinePlusTests.TestProcessors
{
    [ExcludeFromCodeCoverage]
    internal class TestProcessorWithMultipleSubOptionCandidates : BaseCommandLine
    {
        public override string Name => "Option";

        public override int SortOrder => 0;

        public override bool IsEnabled => true;

        public override void DisplayHelp(IDisplay display)
        {
            display.WriteLine("Test");
            display.WriteLine("    Add");
            display.WriteLine("    -a    Option a");
            display.WriteLine("    -b    Option b");
            display.WriteLine("    -c    Option c");
            display.WriteLine("    -d    Option d");
            display.WriteLine("    -e    Option e");
        }

        public override void Execute(string[] args)
        {
            throw new NotImplementedException();
        }

        public void Test(string a)
        {
            ArgsPassed.Add(a);
        }

        public void Test(string a, DateTime b)
        {
            ArgsPassed.Add(a);
            ArgsPassed.Add(b.ToString());
        }

        public void Test(string a, uint b, decimal c)
        {
            ArgsPassed.Add(a);
            ArgsPassed.Add(b.ToString());
            ArgsPassed.Add(c.ToString());
        }

        public void Test(Guid a, string b, string c, bool d = true, byte e = 123)
        {
            ArgsPassed.Add(a.ToString());
            ArgsPassed.Add(b);
            ArgsPassed.Add(c);
            ArgsPassed.Add(d.ToString());
            ArgsPassed.Add(e.ToString());
        }

        public List<string> ArgsPassed { get; private set; } = new();
    }
}
