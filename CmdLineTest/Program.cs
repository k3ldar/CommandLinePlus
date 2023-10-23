
using CmdLineTest;

IConsoleProcessorFactory factory = new ConsoleProcessorFactory();

object[] processors = new object[]
{
    new PluginProcessor(),
};

IConsoleProcessor consoleProcessor = factory.Create("MyProg", processors);

switch (consoleProcessor.Run(out int resultCode))
{
    case RunResult.CandidateFound:
        Console.WriteLine("finished");
        break;

    case RunResult.DisplayHelp:
        break;

    default:
        throw new InvalidOperationException("Didn't work");
}

return resultCode;