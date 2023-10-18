
using CmdLineTest;

IConsoleProcessorFactory factory = new ConsoleProcessorFactory();

object[] processors = new object[]
{
    new PluginProcessor(),
};

IConsoleProcessor consoleProcessor = factory.Create("MyProg", processors);

switch (consoleProcessor.Run())
{
    case RunResult.CandidateFound:
        Console.WriteLine("finished");
        break;

    default:
        throw new InvalidOperationException("Didn't work");
}