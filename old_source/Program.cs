using System.IO;

namespace Suteki
{
    class Program
    {
        static void Main(string[] arguments)
        {
            foreach (string argument in arguments)
            {
                Config.Inputs.Add(new Input(argument, File.ReadAllText(argument)));
            }
            
            Compiler.Start();
            Linker.Start();
        }
    }
}
