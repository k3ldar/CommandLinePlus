using System;
using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CmdLineDescriptionTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            CmdLineDescriptionAttribute sut = new("this paramater is used for...");
            Assert.IsNotNull(sut);
            Assert.AreEqual("this paramater is used for...", sut.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            new CmdLineDescriptionAttribute("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Null_Throws_ArgumentNullException()
        {
            new CmdLineDescriptionAttribute(null);
        }
    }
}
