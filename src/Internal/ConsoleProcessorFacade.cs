using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using static CommandLinePlus.Constants;
using static CommandLinePlus.Properties.Resources;
using static CommandLinePlus.VerbosityLevel;

#pragma warning disable CA2208

namespace CommandLinePlus.Internal
{
    internal sealed class ConsoleProcessorFacade : IConsoleProcessor
    {
        private const int SuccessResponseCode = 0;
        private const int InvalidParameters = Int32.MinValue;
        private const int DefaultSubOptionUsed = InvalidParameters + 1;
        private const int TooManyCandidates = DefaultSubOptionUsed + 1;
        private const int NotEnoughCandidates = TooManyCandidates + 1;
        private const int CmdLineProcessorNotFound = NotEnoughCandidates + 1;
        private static readonly List<string> _ignoreMethods = new() { nameof(BaseCommandLine.DisplayHelp), nameof(BaseCommandLine.Execute) };
        private readonly string _processName;
        private readonly ICommandLineArguments _args;
        private readonly IDisplay _display;
        private readonly object[] _processors;
        private ICommandLineOptions _options;

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
            return Run(out int _);
        }

        public RunResult Run(out int result)
        {
            return Run(new DefaultOptions(), out result);
        }

        public RunResult Run(ICommandLineOptions options, out int result)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _display.WriteLine(Quiet, _processName);
            _display.WriteLine(Diagnostic, DisplayVerbosityLevel, _display.Verbosity);

            List<BaseCommandLine> processors = ValidateCommandLineProcessors();

            if (_args.Contains(CmdLineSettingOptions))
            {
                result = ShowHelpForAllCommandLineProcessors(processors);
                return RunResult.DisplayHelp;
            }
            else
            {
                return FindAndExecuteCommandLineProcessor(processors, out result);
            }
        }

        private RunResult FindAndExecuteCommandLineProcessor(List<BaseCommandLine> processors, out int resultCode)
        {
            StringComparison stringComparer = _options.CaseSensitiveOptionNames ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            var validProcessors = processors.Where(p => p.Name.Equals(_args.PrimaryOption, stringComparer)).ToArray();

            if (validProcessors.Length == 0)
            {
                resultCode = TooManyCandidates;
                return RunResult.NotEnoughCandidates;
            }
            else if (validProcessors.Length > 1)
            {
                resultCode = NotEnoughCandidates;
                return RunResult.TooManyCandidates;
            }

            BaseCommandLine processor = validProcessors[0];

            Type processorType = processor.GetType();

            List<MethodInfo> methods = processorType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.Name.Equals(_args.SubOption, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.GetParameters().Length)
                .ToList();

            // find a matching method that has the right parameters for those passed, include default parameters if missing etc
            RunResult candidateResult = RunResult.None;

            (candidateResult, resultCode) = ValidateMatchingCandidateMethods(processor, methods);

            if (candidateResult == RunResult.InvalidParameters || candidateResult == RunResult.CandidateFound)
            {
                return candidateResult;
            }

            // validate above matching param, if not found fall into below 
            if (methods.Count == 0)
            {
                // no candidate methods found, use default method instead
                var defaultMethod = processorType.GetMethod(nameof(BaseCommandLine.Execute), BindingFlags.Instance | BindingFlags.Public);
                resultCode = (int)defaultMethod.Invoke(processor, new object[] { _args.AllArguments() });
                return RunResult.DefaultSubOptionUsed;
            }

            // should never reach this point!
            throw new InvalidOperationException("No candidates found and deault method was not called");
        }

        private (RunResult, int) ValidateMatchingCandidateMethods(object instance, List<MethodInfo> methods)
        {
            if (methods.Count == 0)
                return (RunResult.DefaultSubOptionUsed, DefaultSubOptionUsed);

            for (int i = methods.Count - 1; i > -1; i--)
            {
                MethodInfo method = methods[i];
                bool isValidCandidate = true;
                List<object> parameters = new();
                List<string> errorList = new();

                // validate each parameter
                foreach (ParameterInfo param in method.GetParameters())
                {
                    CmdLineAbbreviationAttribute abbreviated = param.GetCustomAttribute(typeof(CmdLineAbbreviationAttribute)) as CmdLineAbbreviationAttribute;
                    bool useAbbreviation = abbreviated != null && !_args.Contains(param.Name);
                    string argName = useAbbreviation ? abbreviated.Abbreviation : param.Name;

                    if (_args.Contains(argName))
                    {
                        // validate the param to make sure it can be used
                        try
                        {
                            StringComparison stringComparer = _options.CaseSensitiveParameterNames ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                            if (param.ParameterType.FullName.Equals("System.Guid", stringComparer))
                            {
                                parameters.Add(TypeDescriptor.GetConverter(param.ParameterType).ConvertFromInvariantString(_args.Get<string>(argName)));
                            }
                            else if (param.ParameterType.BaseType.Name.Equals("Enum", stringComparer))
                            {
                                parameters.Add(Enum.Parse(param.ParameterType, _args.Get<string>(argName), true));
                            }
                            else
                            {
                                parameters.Add(Convert.ChangeType(_args.Get<string>(argName), param.ParameterType));
                            }
                        }
                        catch (Exception e)
                            when (e is InvalidCastException ||
                                  e is FormatException ||
                                  e is OverflowException ||
                                  e is ArgumentNullException)
                        {
                            isValidCandidate = false;
                            errorList.Add($"Could not convert argument {argName} ({_args.Get<string>(argName)}) to {param.ParameterType.Name} - {e.Message}");
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
                    object resultCode = method.Invoke(instance, parameters.ToArray()) ?? SuccessResponseCode;
                    return (RunResult.CandidateFound, (int)resultCode);
                }

                methods.Remove(method);

                if (errorList.Count > 0)
                {
                    foreach (string error in errorList)
                    {
                        _display.WriteLine(error);
                    }

                    return (RunResult.InvalidParameters, InvalidParameters);
                }
            }

            throw new NotImplementedException();
        }

        private int ShowHelpForAllCommandLineProcessors(List<BaseCommandLine> processors)
        {
            if (_options.ShowHelpMessage)
                _display.WriteLine(Quiet, string.Format(CultureInfo.InvariantCulture, HelpOption,
                    SetMinimumLength(_options.ParameterPrefix + "-?", _options.InternalOptionsMinimumLength), _options.ParameterSuffix, DisplayHelp));

            if (_options.ShowVerbosity)
                _display.WriteLine(Quiet, string.Format(CultureInfo.InvariantCulture, VerbosityOption,
                    SetMinimumLength(_options.ParameterPrefix + "-v", _options.InternalOptionsMinimumLength), _options.ParameterSuffix, DisplayVerbosity));

            if (_options.ShowHelpMessage || _options.ShowVerbosity)
                _display.WriteLine(Quiet, "\t");

            if (String.IsNullOrEmpty(_args.PrimaryOption))
            {
                return DisplayAllPrimaryOptions(processors);
            }

            StringComparison stringComparerOption = _options.CaseSensitiveOptionNames ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            BaseCommandLine processor = processors.Find(p => p.Name.Equals(_args.PrimaryOption, stringComparerOption));

            if (processor == null)
            {
                _display.WriteLine("Option not found, use -? for help");
                return CmdLineProcessorNotFound;
            }

            if (String.IsNullOrEmpty(_args.SubOption))
            {
                return DisplayAllSubOptionsForProcessor(processor);
            }

            StringComparison stringComparerSubOption = _options.CaseSensitiveSubOptionNames ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            MethodInfo subOption = processor.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.Name.Equals(_args.SubOption, stringComparerSubOption))
                .OrderByDescending(m => m.GetParameters().Length)
                .FirstOrDefault();

            if (subOption == null)
            {
                _display.WriteLine($"Option {_args.SubOption} not found for {processor.Name}, use -? for help");
                return NotEnoughCandidates;
            }

            return DisplayAllParametersForSubOption(processor, subOption);
        }

        private int DisplayAllPrimaryOptions(List<BaseCommandLine> processors)
        {
            foreach (BaseCommandLine processor in processors)
            {
                _display.WriteLine(Quiet, $"{processor.Name}\t\t{GetProcessorDescription(processor)}");
                processor.DisplayHelp();
            }

            return SuccessResponseCode;
        }

        private int DisplayAllSubOptionsForProcessor(BaseCommandLine processor)
        {
            _display.WriteLine(Quiet, processor.Name);
            Type processorType = processor.GetType();
            List<string> namesProcessed = new();

            List<MethodInfo> methods = processorType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding)
                .Where(m => !m.IsSpecialName)
                .ToList();

            methods.RemoveAll(rm => _ignoreMethods.Contains(rm.Name));

            foreach (MethodInfo methodInfo in methods)
            {
                if (namesProcessed.Contains(methodInfo.Name) || methodInfo.GetCustomAttribute(typeof(CmdLineHiddenAttribute)) != null)
                    continue;

                _display.WriteLine(Quiet, $"{_options.SubOptionPrefix}{SetMinimumLength(methodInfo.Name, _options.SubOptionMinimumLength)}{_options.SubOptionSuffix}{GetMethodDescription(methodInfo)}");
                namesProcessed.Add(methodInfo.Name);
            }

            processor.DisplayHelp();

            return SuccessResponseCode;
        }

        private static string SetMinimumLength(string name, int minLength)
        {
            StringBuilder result = new(name, minLength);

            while (result.Length < minLength)
            {
                result.Append(' ');
            }

            return result.ToString();
        }

        private int DisplayAllParametersForSubOption(BaseCommandLine processor, MethodInfo subOption)
        {
            int minLength = _options.ParameterPrefix.Length + _options.ParameterSuffix.Length + _options.ParameterMinimumLength - (processor.Name.Length + subOption.Name.Length);
            _display.WriteLine(Quiet, $"{processor.Name} {SetMinimumLength(subOption.Name, minLength)}{GetMethodDescription(subOption)}");


            foreach (ParameterInfo param in subOption.GetParameters())
            {
                string description = String.Empty;
                string abbreviation = String.Empty;

                CmdLineAbbreviationAttribute abbreviatedAttr = param.GetCustomAttribute(typeof(CmdLineAbbreviationAttribute)) as CmdLineAbbreviationAttribute;

                if (abbreviatedAttr == null)
                {
                    CmdLineDescriptionAttribute descriptionAttr = param.GetCustomAttribute(typeof(CmdLineDescriptionAttribute)) as CmdLineDescriptionAttribute;

                    if (descriptionAttr != null)
                        description = descriptionAttr.Description;
                }
                else
                {
                    description = abbreviatedAttr.Description;
                    abbreviation = abbreviatedAttr.Abbreviation;
                }

                if (!String.IsNullOrEmpty(abbreviation))
                    abbreviation = $"(abbr. -{abbreviation})";

                _display.WriteLine(Quiet, $"{_options.ParameterPrefix}-{SetMinimumLength(param.Name, _options.ParameterMinimumLength)}{_options.ParameterSuffix}{description} {abbreviation}");
            }

            processor.DisplayHelp();

            return SuccessResponseCode;
        }

        private static string GetMethodDescription(MethodInfo methodInfo)
        {
            CmdLineDescriptionAttribute descriptionAttr = methodInfo.GetCustomAttribute(typeof(CmdLineDescriptionAttribute)) as CmdLineDescriptionAttribute;

            if (descriptionAttr == null)
                return String.Empty;

            return descriptionAttr.Description;
        }

        private static string GetProcessorDescription(BaseCommandLine processor)
        {
            CmdLineDescriptionAttribute descriptionAttr = processor.GetType().GetCustomAttribute(typeof(CmdLineDescriptionAttribute)) as CmdLineDescriptionAttribute;

            if (descriptionAttr == null)
                return String.Empty;

            return descriptionAttr.Description;
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
                    baseCommandLine.Update(_args, _display);
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