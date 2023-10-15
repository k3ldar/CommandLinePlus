using System;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus.Abstractions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlus.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CommandLineTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            CommandLineArgs sut = new CommandLineArgs();

            Assert.IsNotNull(sut);

            Assert.IsNotNull(sut as ICommandLineArgs);
        }

        [TestMethod]
        public void Construct_NullArgsConvertsToEmptyStringArray_Success()
        {
            CommandLineArgs sut = new CommandLineArgs(null);
            Assert.AreEqual(0, sut.ArgumentCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Contains_InvalidParamNameNull_Throws_ArgumentNullException()
        {
            CommandLineArgs sut = new CommandLineArgs();
            sut.Contains(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Contains_InvalidParamNameEmptyString_Throws_ArgumentNullException()
        {
            CommandLineArgs sut = new CommandLineArgs();
            sut.Contains("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_InvalidParamNameNull_Throws_ArgumentNullException()
        {
            CommandLineArgs sut = new CommandLineArgs();
            sut.Get<object>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_InvalidParamNameEmptyString_Throws_ArgumentNullException()
        {
            CommandLineArgs sut = new CommandLineArgs();
            sut.Get<object>("");
        }

        [TestMethod]
        public void Contains_CanHandleMultipleSeperatorTypes_ReturnsThreeItems()
        {
            string[] args = new string[] {
                "--test1 test1value",
                "--test2=test2value",
                "/test3:test3value"
            };

            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(3, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test2"));
            Assert.IsTrue(sut.Contains("test3"));
            Assert.AreEqual("test1value", sut.Get<string>("test1"));
            Assert.AreEqual("test2value", sut.Get<string>("test2"));
            Assert.AreEqual("test3value", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Contains_NotFound_ReturnsFalse()
        {
            CommandLineArgs sut = new CommandLineArgs();
            Assert.IsFalse(sut.Contains("test"));
        }

        [TestMethod]
        public void Construct_InvalidParamValue_Ignored()
        {
            string[] args = new string[] {
                "--test1 test1 value",
                "--test2=test2V=alue",
                "/test3:test3va:lue"
            };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(3, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.AreEqual("test1 value", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lue", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_NoSpacesBetweenSeperatorsCommandLineParameters_Success()
        {
            string[] args = new string[] {
                "--test1 test1 value--test2=test2V=alue/test3:test3va:lUE",
            };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(3, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.AreEqual("test1 value", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lUE", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_FindsLastParam_Success()
        {
            string[] args = new string[] {
        "--test1 test1 value--test2=test2V=alue/test3:test3va:lUE--test",
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(4, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.AreEqual("test1 value", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lUE", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_LastValueDoesNotExist_Success()
        {
            string[] args = new string[] {
        "--test1 test1 value--test2=test2V=alue/test3:test3va:lUE--test-",
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(4, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("test"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.IsTrue(sut.Contains("test1"));
            Assert.AreEqual("test1 value", sut.Get<string>("test1"));
            Assert.AreEqual("test2V=alue", sut.Get<string>("test2"));
            Assert.AreEqual("test3va:lUE", sut.Get<string>("test3"));
        }

        [TestMethod]
        public void Construct_AcceptsDoubleQuotesToContainValues_Success()
        {
            string[] args = new string[] {
        "--dq \"-100 --test /forward\""
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("dq"));
            Assert.AreEqual("-100 --test /forward", sut.Get<string>("dq"));
        }

        [TestMethod]
        public void Construct_AcceptsOpenEndedDoubleQuotesToContainValues_Success()
        {
            string[] args = new string[] {
        "--dq \"-100 --test /forward"
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("dq"));
            Assert.AreEqual("-100 --test /forward", sut.Get<string>("dq"));
        }

        [TestMethod]
        public void Construct_SingleDashForNegativeValue_Success()
        {
            string[] args = new string[] {
        "--nv -100"
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("nv"));
            Assert.AreEqual(-100, sut.Get<int>("nv"));
        }

        [TestMethod]
        public void Get_FailToConvertToTypeBool_ReturnsDefaultValueForType()
        {
            string[] args = new string[] {
        "--boolValue test1 value",
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("boolValue"));
            Assert.IsFalse(sut.Get<bool>("boolValue"));
        }

        [TestMethod]
        public void Get_FailToConvertToTypeString_ReturnsDefaultValueForType()
        {
            string[] args = new string[] { };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(0, sut.ArgumentCount);
            Assert.IsFalse(sut.Contains("stringValue"));
            Assert.IsNull(sut.Get<string>("stringValue"));
        }

        [TestMethod]
        public void Get_FailToConvertToTypeInt_ReturnsDefaultValueForType()
        {
            string[] args = new string[] {
        "--IntValue test1 value",
      };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(1, sut.ArgumentCount);
            Assert.IsTrue(sut.Contains("IntValue"));
            Assert.AreEqual(0, sut.Get<int>("IntValue"));
        }

        [TestMethod]
        public void Get_NotFound_ReturnsDefaultValueForType_Bool_Success()
        {
            string[] args = new string[] { };
            CommandLineArgs sut = new CommandLineArgs(args);
            Assert.AreEqual(0, sut.ArgumentCount);
            Assert.IsFalse(sut.Contains("boolValue"));
            Assert.IsFalse(sut.Get<bool>("boolValue"));
        }
    }
}
