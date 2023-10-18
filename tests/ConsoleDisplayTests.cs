using System;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;
using CommandLinePlus.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static CommandLinePlus.VerbosityLevel;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleDisplayTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidArgsNull_Throws_ArgumentNullException()
        {
            new ConsoleDisplay(null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments());
            Assert.IsNotNull(sut);

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteLine_NullMessage_Ignored()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.WriteLine(Quiet, null);

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteLine_EmptyStringMessage_Ignored()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.WriteLine(Quiet, "");

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteLine_ValidString_MessageDisplayed()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.WriteLine(Quiet, "test");

            Assert.AreEqual(1, sut.LineCount);
        }

        [TestMethod]
        public void WriteLine_VerbosityGreaterThanExpected_MessageNotDisplayed()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(new string[] { "--v:0" }));
            sut.WriteLine(Normal, "test");

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteError_NullMessage_Ignored()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.Write(exception: null);

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteError_ValidException_MessageDisplayed()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.Write(new Exception("test"));

            Assert.AreEqual(1, sut.LineCount);
        }

        [TestMethod]
        public void WriteError_ValidException_WithInnerException()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            try
            {
                ThrowNotImplementedException();
            }
            catch (Exception ex)
            {
                sut.Write(ex);
            }

            Assert.AreEqual(3, sut.LineCount);
        }

        [TestMethod]
        public void WriteWarning_NullMessage_Ignored()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.WriteLine((string)null);

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteWarning_EmptyStringMessage_Ignored()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.WriteLine("");

            Assert.AreEqual(0, sut.LineCount);
        }

        [TestMethod]
        public void WriteWarning_ValidString_MessageDisplayed()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(new string[] { }));
            sut.WriteLine("test");

            Assert.AreEqual(1, sut.LineCount);
        }

        [TestMethod]
        public void Verbosity_DefaultNormal_Success()
        {
            string[] args = new string[] { };
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(args));
            Assert.AreEqual(VerbosityLevel.Normal, sut.Verbosity);
        }

        [TestMethod]
        public void Verbosity_SetByArgs_Quiet_Success()
        {
            string[] args = new string[] { "--v:0" };
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(args));
            Assert.AreEqual(VerbosityLevel.Quiet, sut.Verbosity);
        }

        [TestMethod]
        public void Verbosity_SetByArgsNormal_Success()
        {
            string[] args = new string[] { "--v 1" };
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(args));
            Assert.AreEqual(VerbosityLevel.Normal, sut.Verbosity);
        }

        [TestMethod]
        public void Verbosity_SetByArgsDiagnostic_Success()
        {
            string[] args = new string[] { "--v:2" };
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(args));
            Assert.AreEqual(VerbosityLevel.Diagnostic, sut.Verbosity);
        }

        [TestMethod]
        public void Verbosity_SetByArgsInvalidValueStringDefaultsToNormal_Success()
        {
            string[] args = new string[] { "--v asdf" };
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(args));
            Assert.AreEqual(VerbosityLevel.Normal, sut.Verbosity);
        }

        [TestMethod]
        public void Verbosity_SetByArgsInvalidValueIntDefaultsToNormal_Success()
        {
            string[] args = new string[] { "--v 209" };
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments(args));
            Assert.AreEqual(VerbosityLevel.Normal, sut.Verbosity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLine_Format_InvalidParam_NullMessage_Throws_ArgumentNullException()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments());
            sut.WriteLine(VerbosityLevel.Quiet, null, 1, 2, 3);
        }

        [TestMethod]
        public void WriteLine_Format_ValidInput_WrittenToDisplay()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments());
            sut.WriteLine(VerbosityLevel.Quiet, "This is a test {0} {1} {2}", 1, 2, 3);
            Assert.AreEqual(1, sut.LineCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLine_Format_InvalidParam_NullArgs_Throws_ArgumentNullException()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments());
            sut.WriteLine(VerbosityLevel.Quiet, "message", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WriteLine_Format_InvalidMessage_IncorrectlyFormatted_Throws_ArgumentOutOfRangeException()
        {
            ConsoleDisplay sut = new ConsoleDisplay(new CommandLineArguments());
            sut.WriteLine(VerbosityLevel.Quiet, "message", 1, 2, 3);
        }

        [TestMethod]
        public void WriteLine_Format_MessageLogged_Success()
        {
            ConsoleDisplay sut = new ConsoleDisplay(CreateArgs());
            sut.WriteLine("Test: {0} {1}", "one", "two");
            Assert.AreEqual(1, sut.LineCount);

        }

        private void ThrowNotImplementedException()
        {
            throw new NotImplementedException("outer", new Exception());
        }

        private ICommandLineArguments CreateArgs()
        {
            return new CommandLineArguments(new string[] { });
        }
    }
}
