using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using CommandLinePlus;
using CommandLinePlus.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CommandLineArgumentTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            CommandLineArguments
                sut = new CommandLineArguments();

            Assert.IsNotNull(sut);

            Assert.IsNotNull(sut as ICommandLineArguments);
        }

        [TestMethod]
        public void Construct_NullArgsConvertsToEmptyStringArray_Success()
        {
            CommandLineArguments sut = new CommandLineArguments(null);
            Assert.AreEqual(0, sut.ArgumentCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Contains_InvalidParamNameNull_Throws_ArgumentNullException()
        {
            CommandLineArguments sut = new CommandLineArguments();
            sut.Contains(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Contains_InvalidParamNameEmptyString_Throws_ArgumentNullException()
        {
            CommandLineArguments sut = new CommandLineArguments();
            sut.Contains("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_InvalidParamNameNull_Throws_ArgumentNullException()
        {
            CommandLineArguments sut = new CommandLineArguments();
            sut.Get<object>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_InvalidParamNameEmptyString_Throws_ArgumentNullException()
        {
            CommandLineArguments sut = new CommandLineArguments();
            sut.Get<object>("");
        }

        [TestMethod]
        public void Contains_CanHandleMultipleSeperatorTypes_ReturnsThreeItems()
        {
            string[] args = new string[] {
                "-testa testavalue",
                "--testb=testbvalue",
                "/testc:testcvalue"
            };

            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(3, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("testa"));
            Assert.IsTrue(sut.Contains("testb"));
            Assert.IsTrue(sut.Contains("testc"));
            Assert.AreEqual("", sut.Get<string>("testa"));
            Assert.AreEqual("testbvalue", sut.Get<string>("testb"));
            Assert.AreEqual("testcvalue", sut.Get<string>("testc"));
        }

        [TestMethod]
        public void Contains_NotFound_ReturnsFalse()
        {
            CommandLineArguments sut = new CommandLineArguments();
            Assert.IsFalse(sut.Contains("test"));
        }

        [TestMethod]
        public void Construct_InvalidParamValue_Ignored()
        {
            string[] args = new string[] {
                "-test1 test1 value",
                "--test2=test2V=alue",
                "/test3:test3va:lue"
            };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(3, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test2"));
            Assert.IsTrue(sut.Contains("test3"));
            Assert.AreEqual("", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lue", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_NoSpacesBetweenSeperatorsCommandLineParameters_Success()
        {
            string[] args = new string[] {
                "--test1 test1 value--test2=test2V=alue/test3:test3va:lUE",
            };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(3, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test2"));
            Assert.IsTrue(sut.Contains("test3"));
            Assert.AreEqual("", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lUE", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_FindsLastParam_Success()
        {
            string[] args = new string[] {
                "--test1 test1 value--test2=test2V=alue/test3:test3va:lUE--test",
            };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(4, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test2"));
            Assert.IsTrue(sut.Contains("test3"));
            Assert.AreEqual("", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lUE", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_LastValueDoesNotExist_Success()
        {
            string[] args = new string[] {
                "--test1 test1 value--test2=test2V=alue/test3:test3va:lUE--test-",
            };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(4, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test"));
            Assert.IsTrue(sut.Contains("test2"));
            Assert.IsTrue(sut.Contains("test3"));
            Assert.IsTrue(sut.Contains("test"));
            Assert.AreEqual("", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lUE", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_AcceptsDoubleQuotesToContainValues_Success()
        {
            string[] args = new string[] { "--dq:\"-100 --test /forward\"" };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("dq"));
            Assert.AreEqual("-100 --test /forward", sut.Get<string>("dq"));
        }

        [TestMethod]
        public void Construct_AcceptsOpenEndedDoubleQuotesToContainValues_Success()
        {
            string[] args = new string[] { "--dq=\"-100 --test /forward" };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("dq"));
            Assert.AreEqual("-100 --test /forward", sut.Get<string>("dq"));
        }

        [TestMethod]
        public void Construct_SingleDashForNegativeValue_Success()
        {
            string[] args = new string[] { "--nv -100" };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(2, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("nv"));
            Assert.IsTrue(sut.Contains("100"));
            Assert.AreEqual(0, sut.Get<int>("nv"));
        }

        [TestMethod]
        public void Construct_InvalidParamsFound_Success()
        {
            string[] args = new string[] { "--boolValue:true test1 value" };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Get<bool>("boolValue"));
        }

        [TestMethod]
        public void Get_FailToConvertToTypeBool_ReturnsDefaultValueForType()
        {
            string[] args = new string[] { "--boolValue test1 value", };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("boolValue"));
            Assert.IsFalse(sut.Get<bool>("boolValue"));
        }

        [TestMethod]
        public void Get_FailToConvertToTypeString_ReturnsDefaultValueForType()
        {
            string[] args = Array.Empty<string>();
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(0, sut.ArgumentCount);
            Assert.IsFalse(sut.Contains("stringValue"));
            Assert.IsNull(sut.Get<string>("stringValue"));
        }

        [TestMethod]
        public void Get_FailToConvertToTypeInt_ReturnsDefaultValueForType()
        {
            string[] args = new string[] { "--IntValue test1 value", };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("IntValue"));
            Assert.AreEqual(0, sut.Get<int>("IntValue"));
        }

        [TestMethod]
        public void Get_NotFound_ReturnsDefaultValueForType_Bool_Success()
        {
            string[] args = Array.Empty<string>();
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual(0, sut.ArgumentCount);
            Assert.IsFalse(sut.Contains("boolValue"));
            Assert.IsFalse(sut.Get<bool>("boolValue"));
        }

        [TestMethod]
        public void PrimaryOption_Found_Success()
        {
            string[] args = new string[] { "Plugin", "Add", "--q:name" };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual("Plugin", sut.PrimaryOption);
            Assert.AreEqual("Add", sut.SubOption);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("q"));
            Assert.AreEqual("name", sut.Get<string>("q"));
        }

        [TestMethod]
        public void SubOption_NotFound_Success()
        {
            string[] args = new string[] { "Plugin", "--q:name" };
            CommandLineArguments sut = new CommandLineArguments(args);
            Assert.AreEqual("Plugin", sut.PrimaryOption);
            Assert.AreEqual("", sut.SubOption);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("q"));
            Assert.AreEqual("name", sut.Get<string>("q"));
        }
    }
}
