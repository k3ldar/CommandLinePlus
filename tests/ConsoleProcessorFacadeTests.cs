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
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite", new object[] { new TestPluginpProcessor() }, args, mockDisplay);

            sut.Run(out int _);

            Assert.IsTrue(mockDisplay.Lines.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Run_InvalidProcessor_DoesNotDescendFromBaseCommandLine_Throws_InvalidOperationException()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "--?" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite", 
                new object[] 
                { 
                    new InvalidProcessor() 
                }, 
                args, mockDisplay);

            sut.Run(out int _);

            Assert.IsTrue(mockDisplay.Lines.Count > 0);
        }

        [TestMethod]
        public void Run_MoreThanOneCandidateAvailable_Returns_TooManyCandidates()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Plugin Add -p:myplugin" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    new TestPluginpProcessor(),
                    new TestPluginpProcessor(),
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.TooManyCandidates, result);
        }

        [TestMethod]
        public void Run_NotEnoughCandidatesAvailable_Returns_NotEnoughCandidates()
        {
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Test Add -p:myplugin" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    new TestPluginpProcessor(),
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.NotEnoughCandidates, result);
        }

        [TestMethod]
        public void Run_SubOptionNotFound_Returns_DefaultSubOptionUsed()
        {
            TestPluginpProcessor testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Plugin", "test", "-p:myplugin" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.DefaultSubOptionUsed, result);
            Assert.AreEqual(1, testProcessor.ArgsPassedToExecute.Length);
            Assert.AreEqual("p=myplugin", testProcessor.ArgsPassedToExecute[0]);
        }

        [TestMethod]
        public void Run_SubOptionFound_CallsRequiredMethod()
        {
            TestPluginpProcessor testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Plugin", "add", "-p:myplugin" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.CandidateFound, result);
            Assert.AreEqual(1, testProcessor.MethodsCalled.Count);
            Assert.IsTrue(testProcessor.MethodsCalled.Contains("Add"));
            Assert.AreEqual(1, testProcessor.ArgsPassed.Count);
            Assert.AreEqual("myplugin", testProcessor.ArgsPassed[0]);
        }

        [TestMethod]
        public void Run_SubOptionFound_WithOptionalParams_CallsRequiredMethod()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] 
                { 
                    "Option", "Test", 
                    "/a=\"a2f119a9-d030-4025-b122-a00a32288d94\"",
                    "-b:hello",
                    "--c=world"
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.CandidateFound, result);
            Assert.AreEqual(5, testProcessor.ArgsPassed.Count);
            Assert.AreEqual("a2f119a9-d030-4025-b122-a00a32288d94", testProcessor.ArgsPassed[0]);
            Assert.AreEqual("hello", testProcessor.ArgsPassed[1]);
            Assert.AreEqual("world", testProcessor.ArgsPassed[2]);
            Assert.AreEqual("True", testProcessor.ArgsPassed[3]);
            Assert.AreEqual("123", testProcessor.ArgsPassed[4]);
        }

        [TestMethod]
        public void Run_SubOptionFound_FormatException_CallsRequiredMethod()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[]
                {
                    "Option", "Test",
                    "/a=\"not a guid\"",
                    "-b:hello",
                    "--c=world",
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.InvalidParameters, result);
            Assert.AreEqual(6, mockDisplay.Lines.Count);
            Assert.AreEqual("Could not convert argument a (not a guid) to Guid - Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).", mockDisplay.Lines[5]);
        }

        [TestMethod]
        public void Run_SubOptionFound_OverflowException_CallsRequiredMethod()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[]
                {
                    "Option", "Test",
                    "/a=\"a2f119a9-d030-4025-b122-a00a32288d94\"",
                    "-b:hello",
                    "--c=world",
                    "-e:874635"
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.InvalidParameters, result);
            Assert.AreEqual(6, mockDisplay.Lines.Count);
            Assert.AreEqual("Could not convert argument e (874635) to Byte - Value was either too large or too small for an unsigned byte.", mockDisplay.Lines[5]);
        }

        [TestMethod]
        public void Run_ShowHelp_NoPrimaryOption_DisplaysAllSubOptionsWithDescriptions_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[]
                {
                    "--?"
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.DisplayHelp, result);
            Assert.AreEqual(9, mockDisplay.Lines.Count);
            Assert.AreEqual("Quiet Option\t\t", mockDisplay.Lines[8]);
        }

        [TestMethod]
        public void Run_ShowHelp_WithPrimaryOption_DisplaysAllSubOptionsWithDescriptions_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[]
                {
                    " Option --?"
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.DisplayHelp, result);
            Assert.AreEqual(11, mockDisplay.Lines.Count);
            Assert.AreEqual("Full Processor valid Option", mockDisplay.Lines[4]);
            Assert.AreEqual("Quiet    -?                   Displays help information", mockDisplay.Lines[5]);
            Assert.AreEqual("Quiet    -v                   Verbosity 0 = quiet; 1 = normal; 2 = diagnostic; 3 = full e.g. -v:3", mockDisplay.Lines[6]);
            Assert.AreEqual("Quiet \t", mockDisplay.Lines[7]);
            Assert.AreEqual("Quiet Option", mockDisplay.Lines[8]);
            Assert.AreEqual("Quiet   Test                  ", mockDisplay.Lines[9]);
            Assert.AreEqual("Quiet   EnumTest              ", mockDisplay.Lines[10]);
        }

        [TestMethod]
        public void Run_ShowHelp_WithPrimaryAndSubOptions_DisplaysAllParameterItemsWithDescriptions_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[]
                {
                    " Option Test --?"
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.DisplayHelp, result);
            Assert.AreEqual(14, mockDisplay.Lines.Count);
            Assert.AreEqual("Quiet    -?                   Displays help information", mockDisplay.Lines[5]);
            Assert.AreEqual("Quiet    -v                   Verbosity 0 = quiet; 1 = normal; 2 = diagnostic; 3 = full e.g. -v:3", mockDisplay.Lines[6]);
            Assert.AreEqual("Quiet \t", mockDisplay.Lines[7]);
            Assert.AreEqual("Quiet Option Test         ", mockDisplay.Lines[8]);
            Assert.AreEqual("Quiet    -guidArgumentName     (abbr. -a)", mockDisplay.Lines[9]);
            Assert.AreEqual("Quiet    -b                    ", mockDisplay.Lines[10]);
            Assert.AreEqual("Quiet    -c                    ", mockDisplay.Lines[11]);
            Assert.AreEqual("Quiet    -d                    ", mockDisplay.Lines[12]);
            Assert.AreEqual("Quiet    -e                    ", mockDisplay.Lines[13]);
        }

        [TestMethod]
        public void Run_CaseInsensitive_SubOptionFound_OverflowException_CallsRequiredMethod()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[]
                {
                    "option", "test",
                    "/a=\"a2f119a9-d030-4025-b122-a00a32288d94\"",
                    "-b:hello",
                    "--c=world",
                    "-e:23"
                });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("My Program",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            MockOptions mockOptions = new()
            {
                CaseSensitiveOptionNames = false,
                CaseSensitiveSubOptionNames = false,
            };

            RunResult result = sut.Run(mockOptions, out int resultCode);
            Assert.AreEqual(RunResult.CandidateFound, result);
            Assert.AreEqual(0, resultCode);
            Assert.AreEqual(5, mockDisplay.Lines.Count);
            Assert.AreEqual("Full Processor valid Option", mockDisplay.Lines[4]);
        }

        [TestMethod]
        public void Run_SubOptionFound_WithEnum_ParamByName_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Option", "EnumTest", "-e:All" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.CandidateFound, result);
        }

        [TestMethod]
        public void Run_SubOptionFound_WithEnum_ParamByValue_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Option", "EnumTest", "-e:4" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.CandidateFound, result);
        }

        [TestMethod]
        public void Run_SubOptionFound_WithNoParams_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Option", "Show" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.CandidateFound, result);
        }

        [TestMethod]
        public void Run_MultipleProcessors_SubOptionFound_WithNoParams_Success()
        {
            TestProcessorWithMultipleSubOptionCandidates testProcessor = new();
            ICommandLineArguments args = new CommandLineArguments(new string[] { "Option", "Show" });
            MockDisplay mockDisplay = new();
            ConsoleProcessorFacade sut = new("TestSuite",
                new object[]
                {
                    new TestPluginpProcessor(),
                    testProcessor,
                },
                args, mockDisplay);

            RunResult result = sut.Run(out int _);
            Assert.AreEqual(RunResult.CandidateFound, result);
            Assert.AreSame(args, sut.Arguments);
            Assert.AreSame(mockDisplay, sut.Display);
        }
    }
}
