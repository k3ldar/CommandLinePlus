using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using static CommandLinePlus.Constants;
using static CommandLinePlus.Properties.Resources;
using static CommandLinePlus.VerbosityLevel;

#pragma warning disable CA2208

namespace CommandLinePlus.Internal
{
    internal sealed class ConsoleProcessorFacade : IConsoleProcessor
    {
        private readonly string _processName;
        private readonly ICommandLineArguments _args;
        private readonly IDisplay _display;
        private readonly object[] _processors;

        internal ConsoleProcessorFacade(string processName, object[] processors, ICommandLineArguments args, IDisplay display)
        {
            if (string.IsNullOrEmpty(processName))
                throw new ArgumentNullException(nameof(processName));

            if (processors == null || processors.Length == 0)
                throw new ArgumentException(nameof(processors));

            _processName = processName;
            _args = args ?? throw new ArgumentNullException(nameof(args));
            _display = display ?? throw new ArgumentNullException(nameof(display));
            _processors = processors;
        }

        public RunResult Run()
        {
            _display.WriteLine(Quiet, _processName);
            _display.WriteLine(Diagnostic, DisplayVerbosityLevel, _display.Verbosity);
            List<BaseCommandLine> processors = ValidateCommandLineProcessors();

            if (_args.Contains(CmdLineSettingOptions))
            {
                ShowHelpForAllCommandLineProcessors(processors);
                return RunResult.DisplayHelp;
            }
            else
            {
                return FindAndExecuteCommandLineProcessor(processors);
            }
        }

        private RunResult FindAndExecuteCommandLineProcessor(List<BaseCommandLine> processors)
        {
            var validProcessors = processors.Where(p => p.Name.Equals(_args.PrimaryOption, StringComparison.Ordinal)).ToArray();
            
            if (validProcessors.Length == 0)
            {
                return RunResult.NotEnoughCandidates;
            }
            else if (validProcessors.Length > 1)
            {
                return RunResult.TooManyCandidates;
            }

            Type processorType = processors[0].GetType();

            List<MethodInfo> methodInfo = processorType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.Name.Equals(_args.SubOption, StringComparison.OrdinalIgnoreCase))
                .ToList();

             find a matching method that has the right parameters for those passed, include default parameters if missing etc

            // validate above matching param, if not found fall into below if test
            if (methodInfo.Count == 0)
            {
                // no candidate methods found, use default method instead
                var defaultMethod = processorType.GetMethod(nameof(BaseCommandLine.Execute), BindingFlags.Instance | BindingFlags.Public);
                defaultMethod.Invoke(processors[0], new object[] { _args.AllArguments() });
                return RunResult.DefaultSubOptionUsed;
            }

            return RunResult.TooManyCandidates;
        }

        private void ShowHelpForAllCommandLineProcessors(List<BaseCommandLine> processors)
        {
            _display.WriteLine(Quiet, string.Format(CultureInfo.InvariantCulture, HelpOption, DisplayHelp));
            _display.WriteLine(Quiet, VerbosityOption, CmdLineSettingVerbosity);

            foreach (BaseCommandLine processor in processors)
                processor.DisplayHelp(_display);
        }

        private List<BaseCommandLine> ValidateCommandLineProcessors()
        {
            List<BaseCommandLine> result = new();
            _display.WriteLine(Full, FindingProcessors);

            foreach (object processor in _processors)
            {
                Type pType = processor.GetType();

                if (!pType.IsSubclassOf(typeof(BaseCommandLine)))
                    throw new InvalidOperationException($"{pType.Name} does not descend from BaseCommandLine");

                BaseCommandLine baseCommandLine = processor as BaseCommandLine;
                _display.WriteLine(Diagnostic, ProcessorFound, baseCommandLine.Name);

                if (_args.Contains(CmdLineSettingOptions) || baseCommandLine.IsEnabled)
                {
                    _display.WriteLine(Full, ProcessorValid, baseCommandLine.Name);
                    result.Add(baseCommandLine);
                }
                else
                {
                    _display.WriteLine(Full, ProcessorInvalid, baseCommandLine.Name);
                }
            }

            return result.OrderBy(p => p.SortOrder).ToList();
        }
    }
}
#pragma warning restore CA2208