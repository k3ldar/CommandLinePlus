using System.Diagnostics.CodeAnalysis;

namespace CmdLineTest
{
    [CmdLineDescription("Processes plugins for entire application")]
    internal class PluginProcessor : BaseCommandLine
    {
        public enum MachineType
        {
            Unspecified,

            CNC,

            Laser,

            Printer,
        }

        public enum MachineFirmware
        {
            grblv1_1,
        }

        public enum PluginHosts
        {
            None = 0,

            Editor = 1,

            SenderHost = 2,

            Sender = 4,

            Service = 8,

            Any = Editor | SenderHost | Sender | Service,
        }

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


        [CmdLineDescription("Adds a new plugin")]
        public int Add(
            [CmdLineAbbreviation("p", "Name of plugin")] string pluginName,
            [CmdLineAbbreviation("a", "Assembly name for plugin")] string assemblyName,
            [CmdLineAbbreviation("u", "Hosts that can load the plugin")] PluginHosts usage,
            [CmdLineAbbreviation("m", "Type of machine the plugin is targeting.")] MachineType machineType,
            [CmdLineAbbreviation("f", "Firmware the plugin is targeting")] MachineFirmware machineFirmware,
            [CmdLineAbbreviation("e", "Set's enabled state")] bool enabled = true,
            [CmdLineAbbreviation("t", "Indicates that plugin contains tool bar items")] bool showToolbarItems = false,
            [CmdLineAbbreviation("d", "Description")] string description = null)
        {
            if (IsEnabled)
                Display.WriteLine(VerbosityLevel.Quiet, $"Add plugin {pluginName}; Assembly: {assemblyName}; Host: {usage}; Machine: {machineType}; Firmware: {machineFirmware}; Enabled: {enabled}; Toolbar: {showToolbarItems}; Description: {description}");

            return 0;
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
