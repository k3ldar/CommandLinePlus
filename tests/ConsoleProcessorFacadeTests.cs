using System;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;
using CommandLinePlus.Internal;

using CommandLinePlusTests.Mocks;
using CommandLinePlusTests.TestProcessors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleProcessorFacadeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProcessNameNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessorFacade(null, new object[] { new TestPluginpProcessor() }, null, new MockDisplay());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_EmptyNameNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessorFacade("", new object[] { new TestPluginpProcessor() }, null, new MockDisplay());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ArgsNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessorFacade("TestSuite", new object[] { new TestPluginpProcessor() }, null, new MockDisplay());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_DisplayNull_Throws_ArgumnetNullException()
        {
            new ConsoleProcessorFacade("TestSuite", new object[] { new TestPluginpProcessor() }, new CommandLineArguments(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Construct_InvalidParam_NullSearchAssemblies_Throws_ArgumentException()
        {
            new ConsoleProcessorFacade("TestSuite", null, new CommandLineArguments(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Construct_InvalidParam_EmptySearchAssemblies_Throws_ArgumentException()
        {
            new ConsoleProcessorFacade("TestSuite", Array.Empty<object>(), new CommandLineArguments(), null);
        }

        [TestMethod]
        public void Run_HelpOptions_Success()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "--?" });
            MockDisplay mockDisplay = new MockDisplay();
            ConsoleProcessorFacade sut = new ConsoleProcessorFacade("TestSuite", new object[] { new TestPluginpProcessor() }, args, mockDisplay);

            sut.Run();

            Assert.IsTrue(mockDisplay.Lines.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Run_InvalidProcessor_DoesNotDescendFromBaseCommandLine_Throws_InvalidOperationException()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "--?" });
            MockDisplay mockDisplay = new MockDisplay();
            ConsoleProcessorFacade sut = new ConsoleProcessorFacade("TestSuite", 
                new object[] 
                { 
                    new InvalidProcessor() 
                }, 
                args, mockDisplay);

            sut.Run();

            Assert.IsTrue(mockDisplay.Lines.Count > 0);
        }

        [TestMethod]
        public void Run_MoreThanOneCandidateAvailable_Returns_TooManyCandidates()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Plugin Add -p:myplugin" });
            MockDisplay mockDisplay = new MockDisplay();
            ConsoleProcessorFacade sut = new ConsoleProcessorFacade("TestSuite",
                new object[]
                {
                    new TestPluginpProcessor(),
                    new TestPluginpProcessor(),
                },
                args, mockDisplay);

            RunResult result = sut.Run();
            Assert.AreEqual(RunResult.TooManyCandidates, result);
        }

        [TestMethod]
        public void Run_NotEnoughCandidatesAvailable_Returns_NotEnoughCandidates()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Test Add -p:myplugin" });
            MockDisplay mockDisplay = new MockDisplay();
            ConsoleProcessorFacade sut = new ConsoleProcessorFacade("TestSuite",
                new object[]
                {
                    new TestPluginpProcessor(),
                },
                args, mockDisplay);

            RunResult result = sut.Run();
            Assert.AreEqual(RunResult.NotEnoughCandidates, result);
        }

        [TestMethod]
        public void Run_SubOptionNotFound_Returns_DefaultSubOptionUsed()
        {
            TestPluginpProcessor testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Plugin", "test", "-p:myplugin" });
            MockDisplay mockDisplay = new MockDisplay();
            ConsoleProcessorFacade sut = new ConsoleProcessorFacade("TestSuite",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run();
            Assert.AreEqual(RunResult.DefaultSubOptionUsed, result);
            Assert.AreEqual(1, testProcessor.ArgsPassedToExecute.Length);
            Assert.AreEqual("p=myplugin", testProcessor.ArgsPassedToExecute[0]);
        }
    }
}
