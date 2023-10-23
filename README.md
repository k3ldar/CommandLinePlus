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

Create a class which descends from BaseCommandLine, specify the name (primary option) and create methods (each method is a sub option)

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

## Devops
[![CodeQL](https://github.com/k3ldar/CommandLinePlus/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/k3ldar/CommandLinePlus/actions/workflows/codeql-analysis.yml) [![SonarCloud](https://github.com/k3ldar/CommandLinePlus/actions/workflows/SonarCloud.yml/badge.svg)](https://github.com/k3ldar/CommandLinePlus/actions/workflows/SonarCloud.yml) [![.NET](https://github.com/k3ldar/CommandLinePlus/actions/workflows/dotnet.yml/badge.svg)](https://github.com/k3ldar/CommandLinePlus/actions/workflows/dotnet.yml)

[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=vulnerabilities)](https://sonarcloud.io/summary/overall?id=k3ldar_CommandLinePlus) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=bugs)](https://sonarcloud.io/summary/overalloverall?id=k3ldar_CommandLinePlus) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=code_smells)](https://sonarcloud.io/summary/overall?id=k3ldar_CommandLinePlus) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=sqale_index)](https://sonarcloud.io/summary/overall?id=k3ldar_CommandLinePlus)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=reliability_rating)](https://sonarcloud.io/summary/overall?id=k3ldar_CommandLinePlus) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=sqale_rating)](https://sonarcloud.io/summary/overall?id=k3ldar_CommandLinePlus) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=security_rating)](https://sonarcloud.io/summary/overall?id=k3ldar_CommandLinePlus) 

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=k3ldar_CommandLinePlus) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_CommandLinePlus&metric=coverage)](https://sonarcloud.io/summary/new_code?id=k3ldar_CommandLinePlus)

