using System;
using System.Globalization;
using System.Runtime.CompilerServices;

using CommandLinePlus.Abstractions;

using static CommandLinePlus.Constants;

namespace CommandLinePlus.Internal
{
    internal sealed class ConsoleDisplay : IDisplay
    {
        public ConsoleDisplay(ICommandLineArgs arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            Verbosity = (VerbosityLevel)arguments.Get<int>(Constants.CmdLineSettingVerbosity, 1);

            if (!Enum.IsDefined(typeof(VerbosityLevel), Verbosity))
                Verbosity = VerbosityLevel.Normal;
        }

        #region IDisplay Methods

        public void Write(Exception exception)
        {
            if (exception == null)
                return;

            ConsoleColor previousForeground = Console.ForegroundColor;
            ConsoleColor previousBackground = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            InternalWriteLine(exception.Message);

            if (exception.InnerException != null)
            {
                InternalWriteLine(String.Format(CultureInfo.InvariantCulture, DisplayInnerException, exception.InnerException.GetType().Name));
                InternalWriteLine(exception.InnerException.Message);
                LineCount++;
            }

            if (exception.StackTrace != null)
            {
                InternalWriteLine(exception.StackTrace.ToString());
                LineCount++;
            }

            LineCount++;

            Console.BackgroundColor = previousBackground;
            Console.ForegroundColor = previousForeground;
        }

        public void WriteLine(VerbosityLevel verbosityLevel, string message)
        {
            if (String.IsNullOrEmpty(message))
                return;

            if (verbosityLevel > Verbosity)
                return;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            InternalWriteLine(message);
            LineCount++;
        }

        public void WriteLine(string message)
        {
            if (String.IsNullOrEmpty(message))
                return;

            ConsoleColor previousForeground = Console.ForegroundColor;
            ConsoleColor previousBackground = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.White;
            InternalWriteLine(message);
            LineCount++;
            Console.BackgroundColor = previousBackground;
            Console.ForegroundColor = previousForeground;
        }

        public void WriteLine(string message, params object[] args)
        {
            WriteLine(String.Format(CultureInfo.InvariantCulture, message, args));
        }

        public void WriteLine(VerbosityLevel verbosityLevel, string message, params object[] args)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            string formattedMessage = String.Format(CultureInfo.InvariantCulture, message, args);

            if (formattedMessage.Equals(message, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException(nameof(message));

            WriteLine(verbosityLevel, formattedMessage);
        }

        #endregion IDisplay Methods

        public VerbosityLevel Verbosity { get; }

        internal int LineCount { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InternalWriteLine(string data)
        {
#if NET_STANDARD
            Console.WriteLine(
                data.Replace("[tab]", "\t")
                .Replace("\\r", "\r")
                .Replace("\\n", "\n")
                );

#else
            Console.WriteLine(
                data.Replace("[tab]", "\t", StringComparison.OrdinalIgnoreCase)
                .Replace("\\r", "\r", StringComparison.OrdinalIgnoreCase)
                .Replace("\\n", "\n", StringComparison.OrdinalIgnoreCase)
                );
#endif
        }
    }
}
