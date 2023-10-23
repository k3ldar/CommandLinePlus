using System.Diagnostics.CodeAnalysis;

using CommandLinePlus.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class CommandLineOptionsTests
    {
        [TestMethod]
        public void Construct_ValidInstanceWithDefaultOptions_Success()
        {
            DefaultOptions sut = new();
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.ShowVerbosity);
            Assert.IsTrue(sut.ShowHelpMessage);
            Assert.AreEqual("  ", sut.SubOptionPrefix);
            Assert.AreEqual("  ", sut.SubOptionSuffix);
            Assert.AreEqual(20, sut.SubOptionMinimumLength);
            Assert.AreEqual("   ", sut.ParameterPrefix);
            Assert.AreEqual(18, sut.ParameterMinimumLength);
            Assert.AreEqual("  ", sut.ParameterSuffix);
            Assert.AreEqual(22, sut.InternalOptionsMinimumLength);
        }
    }
}
