using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLinePlus
{
    /// <summary>
    /// Base class for any command line processors
    /// </summary>
    public abstract class BaseCommandLine
    {
        /// <summary>
        /// Name of processor, also serves as command line option
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Sort order of processor
        /// </summary>
        public abstract int SortOrder { get; }

        /// <summary>
        /// Shows help information for the class
        /// </summary>
        public abstract void DisplayHelp();
    }
}
