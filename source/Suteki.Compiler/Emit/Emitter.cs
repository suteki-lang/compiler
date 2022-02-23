namespace Suteki.Compiler;

public class Emitter
{
    // Emit C++ code
    public static void Start()
    {
        foreach (Input input in Config.Inputs)
        {
            foreach (Node node in input.Nodes)
                node.Emit(input);
        }
    }
}