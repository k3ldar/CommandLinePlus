using System;

namespace CommandLinePlus
{
    /// <summary>
    /// Command line abbreviation, allows for a parameter description
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CmdLineDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Constructor allowing a description to be used
        /// </summary>
        /// <param name="description">end user description of parameter</param>
        /// <exception cref="ArgumentNullException">Thrown if description is null or empty</exception>
        public CmdLineDescriptionAttribute(string description)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            Description = description;
        }

        /// <summary>
        /// Parameter description
        /// </summary>
        public string Description { get; }
    }
}
