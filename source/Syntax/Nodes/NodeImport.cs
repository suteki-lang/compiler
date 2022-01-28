namespace Suteki
{
    class NodeImport : Node
    {
        public Node ModuleName;

        public override string GetString => ModuleName.GetString;
        public override Token  GetToken  => ModuleName.GetToken;

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.LocalIncludes += $"#include <modules/{ModuleName.GetString}.hpp>\n";
        }
    }
}