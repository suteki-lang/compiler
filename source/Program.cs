using System.IO;

namespace Suteki
{
    class Program
    {
        static void Main(string[] arguments)
        {
            Config.Inputs.Add(new Input(arguments[0], File.ReadAllText(arguments[0])));

            if (arguments.Length > 1)
                Config.Inputs.Add(new Input(arguments[1], File.ReadAllText(arguments[1])));

            Compiler.Start();
            Linker.Start();
        }
    }
}
