using System.Diagnostics.CodeAnalysis;

using CommandLinePlus;

using CommandLinePlusTests.TestProcessors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLinePlusTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleProcessorFactoryTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            ConsoleProcessorFactory sut = new();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void Create_ValidConsoleProcessorFacade_Success()
        {
            ConsoleProcessorFactory factory = new();
            var sut = factory.Create("Test Process", new[] { new TestPluginpProcessor() });
            Assert.IsNotNull(sut);
        }
    }
}
