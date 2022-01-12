using System;
using System.IO;

namespace Suteki
{
    class Program
    {
        static void Main(string[] arguments)
        {
            Config.Inputs.Add(new Input(arguments[0], File.ReadAllText(arguments[0])));
            Compiler.Start();
        }
    }
}
