using System.Collections.Generic;

namespace Suteki
{
    enum OptimizationKind
    {
        None,
    }

    class Config
    {
        public static OptimizationKind OptimizationLevel = OptimizationKind.None;
        public static string           OutputPath        = "tests/out/";
        public static List<Input>      Inputs            = new List<Input>();
        public static bool             HadError          = false;
    }
}