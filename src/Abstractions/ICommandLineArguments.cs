namespace CommandLinePlus
{
#pragma warning disable CA1716
    /// <summary>
    /// Interface for command line options
    /// </summary>
    public interface ICommandLineArguments
    {
        /// <summary>
        /// Primary option from command line
        /// </summary>
        string PrimaryOption { get; }

        /// <summary>
        /// Sub option, optional
        /// </summary>
        string SubOption { get; }

        /// <summary>
        /// Determines whether a command line argument exists or not by name (case insensitive)
        /// </summary>
        /// <param name="name">Name of command line argument</param>
        /// <returns>bool</returns>
        bool Contains(string name);

        /// <summary>
        /// Retrieves a value from a command line argument
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="name">Name of argument</param>
        /// <returns>Value of type T</returns>
        T Get<T>(string name);

        /// <summary>
        /// Retrieves a value and allows for a default to be supplied if not found
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="name">Name of argument</param>
        /// <param name="defaultValue">Default value if not found</param>
        /// <returns>Value of type T</returns>
        T Get<T>(string name, T defaultValue);

        /// <summary>
        /// Retrieves all arguments without primary and sub option
        /// </summary>
        /// <returns>string array</returns>
        string[] AllArguments();

        ///// <summary>
        ///// Returns a list of invalid arguments
        ///// </summary>
        ///// <returns>List&lt;string&gt;</returns>
    }
#pragma warning restore CA1716
}
