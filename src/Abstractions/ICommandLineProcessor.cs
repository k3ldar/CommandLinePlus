namespace CommandLinePlus.Abstractions
{
  /// <summary>
  /// Command line processor interface, each type of processor must
  /// inerit from this interface
  /// </summary>
  public interface ICommandLineProcessor
  {
    /// <summary>
    /// Sort order used for processors
    /// </summary>
    int SortOrder { get; }

    /// <summary>
    /// Indicates whether the processor is valid or not
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Retrieves the name of the processor
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Indicates the processor should show it's available options
    /// </summary>
    void DisplayOptions();

    /// <summary>
    /// Method for processing the command line processor
    /// </summary>
    void Process();
  }
}
