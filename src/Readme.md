# CommandLinePlus
This project started because I needed a command line parser that could:

- Have a primary option
- Have a sub option
- Accept command line parameters

This allows a developer to construct a command line in the following form:

myprog.exe Option SubOption -param:value


#### Supports
- Net7.0
- Netstandard2.0

#### How it works

Create a class which descends from BaseCommandLine, specify the name (primary option) and create methods (each method is a sub option

Add CmdLineDescriptions and CmdLineAbbreviation to methods, properties and processor classes for self describing help information.

In the following example we can these command lines:

```
myprog.exe Plugin /p:myplugin
myprog.exe Plugin Add -p:myplugin
myprog.exe Plugin Remove --p:myplugin
myprog.exe Plugin Disable /p:myplugin
myprog.exe Plugin Enable -p:myplugin
```

```
    [CmdLineDescription("Processes plugins for entire application")]
    internal class PluginProcessor : BaseCommandLine, IDisposable
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
            [CmdLineAbbreviation("p", "Name of the plugin to be added")] string pluginName)
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

        [CmdLineHidden]
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
```
#### Available Paramater seperators
The following characters can be used as param seperators
- = (equals)
- : (colon)
 
#### Available Paramater Idetifiers
The following are used to identify parameter values
 - (-) (single dash) doesn't need the brackets except in markdown :-\
 - -- (double dash)
 - / (forward slash)