using System.Diagnostics.CodeAnalysis;

namespace CmdLineTest
{
    [CmdLineDescription("Processes plugins for entire application")]
    internal class PluginProcessor : BaseCommandLine
    {
        public override string Name => "Plugin";

        public override int SortOrder => 0;

        public override bool IsEnabled => true;

        public override void DisplayHelp()
        {

        }

        public override int Execute(string[] args)
        {
            return 0;
        }

        [CmdLineDescription("Adds a new plugin to the application")]
        public void Add(
            [CmdLineAbbreviation("p")][CmdLineDescription("Name of the plugin to be added")] string pluginName)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Add plugin {pluginName}");
        }

        [CmdLineDescription("Removes an existing plugin from the application")]
        public void Remove(
            [CmdLineAbbreviation("p", "Name of the plugin to be removed")] string pluginName)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Remove plugin {pluginName}");
        }

        [CmdLineDescription("Disables a plugin from being used by the application")]
        public void Disable(
            [CmdLineAbbreviation("p", "Name of the plugin to be disabled")] string pluginName)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Disable plugin {pluginName}");
        }

        [CmdLineDescription("Enables a plugin within the application")]
        public void Enable(
            [CmdLineAbbreviation("p", "Name of the plugin to be enabled")] string pluginName)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Enable plugin {pluginName}");
        }

        [CmdLineDescription("Updates a plugins configuration")]
        public void Update(
            [CmdLineDescription("Name of the plugin to be enabled")] string pluginName)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Enable plugin {pluginName}");
        }

        [CmdLineDescription("Updates a plugins configuration")]
        public void Update(
            [CmdLineAbbreviation("p", "Name of the plugin to be enabled")] string pluginName,
            [CmdLineDescription("Boolean option A")] bool optionA,
            [CmdLineDescription("Int options B")] int optionB)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Enable plugin {pluginName}; Option A: {optionA}; Options B: {optionB}");
        }
    }
}
