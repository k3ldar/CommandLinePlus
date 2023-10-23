using System;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CmdLineAbbreviateTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            CmdLineAbbreviationAttribute sut = new("a");
            Assert.IsNotNull(sut);
            Assert.AreEqual("a", sut.Abbreviation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            new CmdLineAbbreviationAttribute("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Null_Throws_ArgumentNullException()
        {
            new CmdLineAbbreviationAttribute(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_WithDescription_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            new CmdLineAbbreviationAttribute("p", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_WithDescription_InvalidParam_Null_Throws_ArgumentNullException()
        {
            new CmdLineAbbreviationAttribute("p", null);
        }

        [TestMethod]
        public void Construct_WithDescription_Success()
        {
            CmdLineAbbreviationAttribute sut = new("p", "Description for param");
            Assert.IsNotNull(sut);
            Assert.AreEqual("p", sut.Abbreviation);
            Assert.AreEqual("Description for param", sut.Description);
        }
    }
}
