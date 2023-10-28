using System;

namespace CommandLinePlus
{
    /// <summary>
    /// Hides a command line from display within help
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CmdLineHiddenAttribute : Attribute
    {
    }
}
