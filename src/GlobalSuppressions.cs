// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Processed in other case", Scope = "member", Target = "~M:CommandLinePlus.CommandLineArgs.ConvertArgsToDictionary(System.String[])~System.Collections.Generic.Dictionary{System.String,System.String}")]
[assembly: SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "Processed in other case", Scope = "member", Target = "~M:CommandLinePlus.CommandLineArgs.ConvertArgsToDictionary(System.String[])~System.Collections.Generic.Dictionary{System.String,System.String}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Required when converting", Scope = "member", Target = "~M:CommandLinePlus.CommandLineArgs.Get``1(System.String,``0)~``0")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:CommandLinePlus.CommandLineArgs.Get``1(System.String,``0)~``0")]
