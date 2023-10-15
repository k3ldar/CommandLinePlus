using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using CommandLinePlus.Abstractions;

using static CommandLinePlus.Constants;
using static CommandLinePlus.Properties.Resources;
using static CommandLinePlus.VerbosityLevel;

#pragma warning disable CA2208

namespace CommandLinePlus
{
    internal sealed class ConsoleProcessor : IConsoleProcessor
    {
        private readonly string _processName;
        private readonly ICommandLineArgs _args;
        private readonly IDisplay _display;
        private readonly Assembly[] _searchAssemblies;

        internal ConsoleProcessor(string processName, ICommandLineArgs args, IDisplay display, Assembly[] assemblies)
        {
            if (String.IsNullOrEmpty(processName))
                throw new ArgumentNullException(nameof(processName));

            if (assemblies == null || assemblies.Length == 0)
                throw new ArgumentException(nameof(assemblies));

            _processName = processName;
            _args = args ?? throw new ArgumentNullException(nameof(args));
            _display = display ?? throw new ArgumentNullException(nameof(display));
            _searchAssemblies = assemblies;
        }

        public void Run()
        {
            _display.WriteLine(Quiet, _processName);
            _display.WriteLine(Diagnostic, DisplayVerbosityLevel, _display.Verbosity);

            List<ICommandLineProcessor> processors = LoadCommandLineProcessors();

            if (_args.Contains(CmdLineSettingOptions))
            {
                _display.WriteLine(Quiet, String.Format(CultureInfo.InvariantCulture, HelpOption, DisplayHelp));
                _display.WriteLine(Quiet, VerbosityOption, CmdLineSettingVerbosity);

                foreach (ICommandLineProcessor processor in processors)
                    processor.DisplayOptions();
            }
        }

        private List<ICommandLineProcessor> LoadCommandLineProcessors()
        {
            _display.WriteLine(Diagnostic, FindingProcessors);

            var result = new List<ICommandLineProcessor>();
            object[] ctorParams = new object[] { _args, _display };

            for (int i = 0; i < _searchAssemblies.Length; i++)
            {
                foreach (Type t in _searchAssemblies[i].GetTypes())
                {
                    if (t.GetInterface(nameof(ICommandLineProcessor)) != null)
                    {
                        _display.WriteLine(Diagnostic, ProcessorFound, t.Name);

                        var processor = (ICommandLineProcessor)Activator.CreateInstance(t, ctorParams);

                        if (_args.Contains(CmdLineSettingOptions) || processor.IsValid)
                        {
                            _display.WriteLine(Diagnostic, ProcessorValid, t.Name);
                            result.Add(processor);
                        }
                        else
                        {
                            _display.WriteLine(Diagnostic, ProcessorInvalid, t.Name);
                        }
                    }
                }
            }

            return result.OrderBy(p => p.SortOrder).ToList();
        }
    }
}
#pragma warning restore CA2208