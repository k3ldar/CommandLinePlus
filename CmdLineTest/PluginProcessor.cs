namespace CmdLineTest
{
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
            // default handler 
        }

        public void Add(string p)
        {
            Console.WriteLine($"Add plugin {p}");
        }

        public void Remove(string p)
        {
            Console.WriteLine($"Remove plugin {p}");
        }

        public void Disable(string p)
        {
            Console.WriteLine($"Disable plugin {p}");
        }

        public void Enable(string p)
        {
            Console.WriteLine($"Enable plugin {p}");
        }
    }
}
