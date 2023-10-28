using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;

namespace CommandLinePlusTests.TestProcessors
{
    [ExcludeFromCodeCoverage]
    internal sealed class TestProcessorWithMultipleSubOptionCandidates : BaseCommandLine, IDisposable
    {
        [Flags]
        public enum MyEnumValues { None = 0, One = 1, Two = 2, Four = 4, All = One | Two | Four };

        public override string Name => "Option";

        public override int SortOrder => 0;

        public override bool IsEnabled => true;

        public override void DisplayHelp()
        {
            // required to be overridden
        }

        public override int Execute(string[] args)
        {
            return -10;
        }

        public int Test(string a)
        {
            ArgsPassed.Add(a);
            return -1;
        }

        public int Test(string a, DateTime b)
        {
            ArgsPassed.Add(a);
            ArgsPassed.Add(b.ToString());
            return 1;
        }

        public int Test(string a, uint b, decimal c)
        {
            ArgsPassed.Add(a);
            ArgsPassed.Add(b.ToString());
            ArgsPassed.Add(c.ToString());
            return 0;
        }

        public int Test([CmdLineAbbreviation("a")] Guid guidArgumentName, string b, string c, bool d = true, byte e = 123)
        {
            ArgsPassed.Add(guidArgumentName.ToString());
            ArgsPassed.Add(b);
            ArgsPassed.Add(c);
            ArgsPassed.Add(d.ToString());
            ArgsPassed.Add(e.ToString());
            return 0;
        }

        public int EnumTest([CmdLineAbbreviation("e")] MyEnumValues myEnum)
        {
            ArgsPassed.Add(myEnum.ToString());
            return 0;
        }

        [CmdLineHidden]
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<string> ArgsPassed { get; private set; } = new();
    }
}
