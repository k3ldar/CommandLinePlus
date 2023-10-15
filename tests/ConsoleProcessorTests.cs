using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using CommandLinePlus.Abstractions;

using CommandLinePlusTests.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlus.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConsoleProcessorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProcessNameNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessor(null, null, new MockDisplay(), new Assembly[] { Assembly.GetExecutingAssembly() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_EmptyNameNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessor("", null, new MockDisplay(), new Assembly[] { Assembly.GetExecutingAssembly() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ArgsNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessor("TestSuite", null, new MockDisplay(), new Assembly[] { Assembly.GetExecutingAssembly() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_DisplayNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessor("TestSuite", new CommandLineArgs(), null, new Assembly[] { Assembly.GetExecutingAssembly() });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_NullSearchAssemblies_Throws_ArgumnetNullException()
        {
            new ConsoleProcessor("TestSuite", new CommandLineArgs(), null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_EmptySearchAssemblies_Throws_ArgumnetNullException()
        {
            new ConsoleProcessor("TestSuite", new CommandLineArgs(), null, new Assembly[] {  });
        }

        [TestMethod]
        public void Execute_HelpOptions_Success()
        {
            ICommandLineArgs args = new CommandLineArgs(new string[] { "--?" });
            MockDisplay mockDisplay = new MockDisplay();
            ConsoleProcessor sut = new ConsoleProcessor("TestSuite", args, mockDisplay, new[] { Assembly.GetExecutingAssembly() });

            sut.Run();

            Assert.IsTrue(mockDisplay.Lines.Count > 30);
        }
    }
}
