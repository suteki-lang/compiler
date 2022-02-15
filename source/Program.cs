using System.IO;

namespace Suteki
{
    class Program
    {
        static void Main(string[] arguments)
        {   
            foreach (string argument in arguments)
            {
                Config.Inputs.Add(new Input(argument, File.ReadAllText(argument) + '\0'));
            }

            Parser parser = new Parser();
            parser.Start();

            if (!Config.HadError)
            {
                Emitter.Start();
                Linker.Start();
            }
        }
    }
}