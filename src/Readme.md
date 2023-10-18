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