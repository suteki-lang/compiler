namespace Suteki
{
    class Compiler
    {
        // Start the compiler
        public static void Start()
        {
            // Parse source code
            Parser parser = new Parser();
            parser.Start();

            // Only emit if no error
            if (Config.HadError)
                return;

            // Emit C++ code
            foreach (Input input in Config.Inputs)
            {
                foreach (Node node in input.Nodes)
                {
                    node.Emit(input);
                }
            }
        }
    }
}