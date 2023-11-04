// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Required when converting", Scope = "member", Target = "~M:CommandLinePlus.Internal.CommandLineArguments.Get``1(System.String,``0)~``0")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Not required at this point", Scope = "member", Target = "~M:CommandLinePlus.Internal.CommandLineArguments.Get``1(System.String,``0)~``0")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Not required at this point", Scope = "member", Target = "~M:CommandLinePlus.Internal.ConsoleProcessorFacade.ValidateMatchingCandidateMethods(System.Object,System.Collections.Generic.List{System.Reflection.MethodInfo})~System.ValueTuple{CommandLinePlus.RunResult,System.Int32}")]
[assembly: SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Above call to ValidateMatchingCandidateMethods will set enum", Scope = "member", Target = "~M:CommandLinePlus.Internal.ConsoleProcessorFacade.FindAndExecuteCommandLineProcessor(System.Collections.Generic.List{CommandLinePlus.BaseCommandLine},System.Int32@)~CommandLinePlus.RunResult")]
[assembly: SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "Above call to ValidateMatchingCandidateMethods will set enum", Scope = "member", Target = "~M:CommandLinePlus.Internal.ConsoleProcessorFacade.FindAndExecuteCommandLineProcessor(System.Collections.Generic.List{CommandLinePlus.BaseCommandLine},System.Int32@)~CommandLinePlus.RunResult")]
