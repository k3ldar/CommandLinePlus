using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

using static System.Net.Mime.MediaTypeNames;
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

            List<MethodInfo> methods = processorType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.Name.Equals(_args.SubOption, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.GetParameters().Length)
                .ToList();

            // find a matching method that has the right parameters for those passed, include default parameters if missing etc
            RunResult candidateResult = ValidateMatchingCandidateMethods(processors[0], methods);

            if (candidateResult == RunResult.InvalidParameters || candidateResult == RunResult.CandidateFound)
                return candidateResult;

            // validate above matching param, if not found fall into below 
            if (methods.Count == 0)
            {
                // no candidate methods found, use default method instead
                var defaultMethod = processorType.GetMethod(nameof(BaseCommandLine.Execute), BindingFlags.Instance | BindingFlags.Public);
                defaultMethod.Invoke(processors[0], new object[] { _args.AllArguments() });
                return RunResult.DefaultSubOptionUsed;
            }

            // should never reach this point!
            throw new InvalidOperationException("No candidates found and deault method was not called");
        }

        private RunResult ValidateMatchingCandidateMethods(object instance, List<MethodInfo> methods)
        {
            if (methods.Count == 0)
                return RunResult.DefaultSubOptionUsed;

            for (int i = methods.Count -1; i > -1; i--)
            {
                MethodInfo method = methods[i];
                bool isValidCandidate = true;
                List<object> parameters = new();
                List<string> errorList = new();

                // validate each parameter
                foreach (var param in method.GetParameters())
                {
                    if (_args.Contains(param.Name))
                    {
                        // validate the param to make sure it can be used
                        try
                        {
                            if (param.ParameterType.FullName.Equals("System.Guid", StringComparison.Ordinal))
                            {
                                parameters.Add(TypeDescriptor.GetConverter(param.ParameterType).ConvertFromInvariantString(_args.Get<string>(param.Name)));
                            }
                            else
                            {
                                parameters.Add(Convert.ChangeType(_args.Get<string>(param.Name), param.ParameterType));
                            }
                        }
                        catch (Exception e)
                            when (e is InvalidCastException || 
                                  e is FormatException ||
                                  e is OverflowException ||
                                  e is ArgumentNullException)
                        {
                            isValidCandidate = false;
                            errorList.Add($"Could not convert argument {param.Name} ({_args.Get<string>(param.Name)}) to {param.ParameterType.Name} - {e.Message}");
                        }
                    }
                    else if (param.IsOptional && param.HasDefaultValue)
                    {
                        parameters.Add(param.DefaultValue);
                    }
                    else
                    {
                        isValidCandidate = false;
                    }
                }

                if (isValidCandidate)
                {
                    method.Invoke(instance, parameters.ToArray());
                    return RunResult.CandidateFound;
                }

                methods.Remove(method);

                if (errorList.Count > 0)
                {
                    foreach(string error in errorList)
                    {
                        _display.WriteLine(error);
                    }

                    return RunResult.InvalidParameters;
                }
            }

            throw new NotImplementedException();
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