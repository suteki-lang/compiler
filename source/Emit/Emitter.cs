namespace Suteki
{
    class Emitter
    {
        // Emit C++ code
        public static void Start(Input input)
        {
            foreach (Node node in input.Nodes)
                node.Emit(input);
        }
    }
}