using System;

namespace CommandLinePlus
{
    /// <summary>
    /// Command line abbreviation, allows a parameter to have a long form (as described by the param name)
    /// or a short form
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CmdLineAbbreviationAttribute : Attribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="abbreviation">abbreviated cmd line parameter name</param>
        /// <exception cref="ArgumentNullException">Thrown if abbreviation is null or empty</exception>
        public CmdLineAbbreviationAttribute(string abbreviation)
        {
            if (String.IsNullOrEmpty(abbreviation))
                throw new ArgumentNullException(nameof(abbreviation));

            Abbreviation = abbreviation;
        }

        /// <summary>
        /// Constructor with parameter description
        /// </summary>
        /// <param name="abbreviation">abbreviated cmd line parameter name</param>
        /// <param name="description">Description for parameter</param>
        /// <exception cref="ArgumentNullException">Thrown if abbreviation is null or empty</exception>
        public CmdLineAbbreviationAttribute(string abbreviation, string description)
            : this(abbreviation)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            Description = description;
        }

        /// <summary>
        /// Abbreviated name
        /// </summary>
        public string Abbreviation { get; }

        /// <summary>
        /// Parameter description shown with help parameter
        /// </summary>
        public string Description { get; }
    }
}
