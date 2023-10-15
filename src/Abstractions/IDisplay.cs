using System;

namespace CommandLinePlus.Abstractions
{
  /// <summary>
  /// Interface used for displaying information to the user
  /// </summary>
  public interface IDisplay
  {
    /// <summary>
    /// Writes an exception to the display
    /// </summary>
    /// <param name="exception"></param>
    void Write(Exception exception);

    /// <summary>
    /// Writes a line of text allowing for verbosity level
    /// </summary>
    /// <param name="verbosityLevel">Verbosity level to display the text</param>
    /// <param name="message">Text to be displayed</param>
    void WriteLine(VerbosityLevel verbosityLevel, string message);

    /// <summary>
    /// Writes a line of text that can be formatted with param arg values
    /// </summary>
    /// <param name="verbosityLevel"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    void WriteLine(VerbosityLevel verbosityLevel,string message,  params object[] args);

    /// <summary>
    /// Writes a warning message to the display
    /// </summary>
    /// <param name="message">Warning message to be displayed</param>
    void WriteLine(string message);


    /// <summary>
    /// Writes a warning message that can be formatted with param arg values
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    void WriteLine(string message, params object[] args);

    /// <summary>
    /// Retrieves the current verbosity level
    /// </summary>
    VerbosityLevel Verbosity { get; }
  }
}
