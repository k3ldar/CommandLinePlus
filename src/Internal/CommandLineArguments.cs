using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using static CommandLinePlus.Constants;

namespace CommandLinePlus.Internal
{
    internal sealed class CommandLineArguments : ICommandLineArguments
    {
        #region Private Members

        private readonly Dictionary<string, string> _args;
        private readonly string _primaryOption;
        private readonly string _subOption;

        #endregion Private Members

        #region Constructors

        public CommandLineArguments()
          : this(Environment.GetCommandLineArgs())
        {

        }

        public CommandLineArguments(string[] args)
        {
            (_primaryOption, _subOption, _args) = ConvertArgsToDictionary(args ?? Array.Empty<string>());
        }

        #endregion Constructors

        #region Properties

        public string PrimaryOption => _primaryOption;

        public string SubOption => _subOption;

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

        public string[] AllArguments()
        {
            List<string> args = new();

            foreach (var item in _args)
            {
                args.Add($"{item.Key}={item.Value}");
            }

            return args.ToArray();
        }

        #endregion ICommandLine Methods

        #region Private Methods

        private static (string, string, Dictionary<string, string>) ConvertArgsToDictionary(string[] args)
        {
            string primaryOption = "";
            string subOption = "";
            var result = new Dictionary<string, string>();
            var currentArg = new StringBuilder(30);
            var currentArgValue = new StringBuilder();

            string arg = string.Join(" ", args);

            bool peekAhead;
            bool argFound = false;
            bool argValue = false;
            bool isQuote = false;
            bool isPrimary = true;
            bool isSub = false;

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

                        if (isPrimary || isSub)
                        {
                            isPrimary = false;
                            isSub = false;
                        }

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
                        else if (arg[i] == CharDash || (peekAhead && arg[i + 1] == CharDash))
                        {
                            argFound = true;
                            argValue = false;
                            
                            if (peekAhead && arg[i + 1] == CharDash)
                                i++;
                        }

                        continue;

                    case CharSpace:
                        if (isPrimary)
                        {
                            primaryOption = currentArg.ToString();
                            currentArg.Clear();
                            isSub = true;
                            isPrimary = false;
                        }
                        else if (isSub)
                        {
                            subOption = currentArg.ToString();
                            currentArg.Clear();
                            isSub = false;
                        }
                        else if (argFound && !argValue)
                        {
                            argValue = true;
                        }
                        else if (argValue)
                        {
                            currentArgValue.Append(c);
                        }

                        continue;

                    case CharColon:
                    case CharEquals:
                        if (argFound && !argValue)
                            argValue = true;
                        else if (argValue)
                            currentArgValue.Append(c);

                        continue;

                    default:
                        if (isPrimary || isSub)
                        {
                            currentArg.Append(c);
                        }
                        else if (argValue)
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

            return (primaryOption, subOption, result);
        }

        #endregion Private Methods
    }
}
