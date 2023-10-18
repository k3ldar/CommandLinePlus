using System;
using System.Collections.Generic;
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

            StringBuilder arg = new(1024);

            foreach (string s in args)
            {
                if (arg.Length > 0)
                    arg.Append(' ');

                arg.Append(s.Trim());
            }

            bool peekAhead;
            bool isArgFound = false;
            bool isArgValue = false;
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
                        isArgValue = isQuote;

                        if (!isQuote)
                        {
                            result.Add(currentArg.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture), currentArgValue.ToString().Trim());
                            currentArg.Clear();
                            currentArgValue.Clear();
                            isArgFound = false;
                            isArgValue = false;
                        }
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

                        if (isQuote && isArgValue)
                        {
                            currentArgValue.Append(c);
                            continue;
                        }

                        if (isArgFound && isArgValue && arg[i + 1] != CharDash && c != CharForwardSlash)
                        {
                            isArgValue = true;
                            currentArgValue.Append(c);
                            continue;
                        }

                        if (isArgValue)
                        {
                            result.Add(currentArg.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture), currentArgValue.ToString().Trim());
                            currentArg.Clear();
                            currentArgValue.Clear();
                            isArgFound = false;
                            isArgValue = false;
                        }

                        if (isArgFound)
                            continue;

                        if (arg[i] == CharForwardSlash)
                        {
                            isArgFound = true;
                            isArgValue = false;
                        }
                        else if (arg[i] == CharDash || (peekAhead && arg[i + 1] == CharDash))
                        {
                            isArgFound = true;
                            isArgValue = false;

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
                        else if (isArgValue && isQuote)
                        {
                            currentArgValue.Append(c);
                        }
                        else if (currentArg.Length > 0)
                        {
                            result.Add(currentArg.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture), currentArgValue.ToString().Trim());
                            currentArg.Clear();
                            currentArgValue.Clear();
                            isArgFound = false;
                            isArgValue = false;
                        }

                        continue;

                    case CharColon:
                    case CharEquals:
                        if (isArgFound && !isArgValue)
                            isArgValue = true;
                        else if (isArgValue)
                            currentArgValue.Append(c);

                        continue;

                    default:
                        if (isPrimary || isSub)
                        {
                            currentArg.Append(c);
                        }
                        else if (isArgValue)
                        {
                            currentArgValue.Append(c);
                            continue;
                        }
                        else if (isArgFound)
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
