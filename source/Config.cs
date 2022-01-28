using System.Collections.Generic;

namespace Suteki
{
    class Config
    {
        public static bool        HadError               = false;
        public static string      OutputPath             = "../tests/out/";
        public static List<Input> Inputs                 = new List<Input>();
        public static Dictionary<string, Module> Modules = new Dictionary<string, Module>();
    }
}