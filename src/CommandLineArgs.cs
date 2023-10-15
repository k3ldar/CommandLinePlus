using System;
using System.Collections.Generic;
using System.Text;

using CommandLinePlus.Abstractions;

using static CommandLinePlus.Constants;

namespace CommandLinePlus
{
    internal sealed class CommandLineArgs : ICommandLineArgs
    {
        #region Private Members

        private readonly Dictionary<string, string> _args;

        #endregion Private Members

        #region Constructors

        public CommandLineArgs()
          : this(Environment.GetCommandLineArgs())
        {

        }

        public CommandLineArgs(string[] args)
        {
            _args = ConvertArgsToDictionary(args ?? Array.Empty<string>());
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Number of arguments supplied via constructor
        /// </summary>
        /// <exception cref="ArgumentNullException">If name is null or empty</exception>
        internal int ArgumentCount => _args.Count;

        #endregion Properties

        #region ICommandLine Methods

        public bool Contains(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return _args.ContainsKey(name.ToLower(System.Globalization.CultureInfo.CurrentCulture));
        }

        public T Get<T>(string name)
        {
            return Get<T>(name, default);
        }

        /// <summary>
        /// Gets a command line argument as specific type, returns default type if failed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T Get<T>(string name, T defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            name = name.ToLower(System.Globalization.CultureInfo.CurrentCulture);

            if (!Contains(name))
                return defaultValue;

            try
            {
                return (T)Convert.ChangeType(_args[name], typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion ICommandLine Methods

        #region Private Methods

        private static Dictionary<string, string> ConvertArgsToDictionary(string[] args)
        {
            var result = new Dictionary<string, string>();
            var currentArg = new StringBuilder(30);
            var currentArgValue = new StringBuilder();

            string arg = string.Join(" ", args);

            bool argFound = false;
            bool argValue = false;
            bool peekAhead = false;
            bool isQuote = false;

            for (int i = 0; i < arg.Length; i++)
            {
                peekAhead = i < arg.Length - 1;
                char c = arg[i];

                switch (c)
                {
                    case CharDblQuotes:
                        isQuote = !isQuote;
                        argValue = isQuote;

                        continue;

                    case CharForwardSlash:
                    case CharDash:
                        if (!peekAhead)
                            continue;

                        if (isQuote && argValue)
                        {
                            currentArgValue.Append(c);
                            continue;
                        }

                        if (argFound && argValue && arg[i + 1] != CharDash && c != CharForwardSlash)
                        {
                            argValue = true;
                            currentArgValue.Append(c);
                            continue;
                        }

                        if (argValue)
                        {
                            result.Add(currentArg.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture), currentArgValue.ToString().Trim());
                            currentArg.Clear();
                            currentArgValue.Clear();
                            argFound = false;
                            argValue = false;
                        }

                        if (argFound)
                            continue;

                        if (arg[i] == CharForwardSlash)
                        {
                            argFound = true;
                            argValue = false;
                        }
                        else if (arg[i + 1] == CharDash)
                        {
                            argFound = true;
                            argValue = false;
                            i++;
                        }

                        continue;

                    case CharSpace:
                    case CharColon:
                    case CharEquals:
                        if (argFound && !argValue)
                            argValue = true;
                        else if (argValue)
                            currentArgValue.Append(c);

                        continue;

                    default:
                        if (argValue)
                        {
                            currentArgValue.Append(c);
                            continue;
                        }
                        else if (argFound)
                        {
                            currentArg.Append(c);
                            continue;
                        }

                        continue;
                }
            }

            if (currentArg.Length > 0)
                result[currentArg.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)] = currentArgValue.ToString().Trim();

            return result;
        }

        #endregion Private Methods
    }
}
