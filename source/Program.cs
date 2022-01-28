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

            Parser parser = new Parser();
            parser.Start();
            Emitter.Start();
            Linker.Start();
        }
    }
}