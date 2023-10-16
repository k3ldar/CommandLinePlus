using System;
using System.Collections.Generic;

using CommandLinePlus;

namespace CommandLinePlusTests.Mocks
{
    internal class MockDisplay : IDisplay
    {
        public List<string> Lines = new List<string>();

        public VerbosityLevel Verbosity { get; set; }

        public void Write(Exception exception)
        {
            Lines.Add($"Exception: {exception}");
        }

        public void WriteLine(VerbosityLevel verbosityLevel, string message)
        {
            Lines.Add($"{verbosityLevel} {message}");
        }

        public void WriteLine(VerbosityLevel verbosityLevel, string message, params object[] args)
        {
            Lines.Add($"{verbosityLevel} {String.Format(message, args)}");
        }

        public void WriteLine(string message)
        {
            Lines.Add(message);
        }

        public void WriteLine(string message, params object[] args)
        {
            Lines.Add(String.Format(message, args));
        }
    }
}
