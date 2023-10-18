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


In the following example we can these command lines:

```
myprog.exe Plugin /p:myplugin
myprog.exe Plugin Add -p:myplugin
myprog.exe Plugin Remove --p:myplugin
myprog.exe Plugin Disable /p:myplugin
myprog.exe Plugin Enable -p:myplugin
```

```
    internal class PluginProcessor : BaseCommandLine
    {
        public override string Name => "Plugin";

        public override int SortOrder => 0;

        public override bool IsEnabled => true;

        public override void DisplayHelp(IDisplay display)
        {
            display.WriteLine("Plugin");
            display.WriteLine("    Add");
            display.WriteLine("    Remove");
            display.WriteLine("    Disable");
            display.WriteLine("    Enable");
            display.WriteLine("    -p    Name of plugin");
        }

        public override void Execute(string[] args)
        {
            // default handler when no sub option specified
        }

        public void Add(string p)
        {

        }

        public void Remove(string p)
        {

        }

        public void Disable(string p)
        {

        }

        public void Enable(string p)
        {

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

