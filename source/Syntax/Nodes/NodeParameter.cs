using System.Collections.Generic;

namespace Suteki
{
    class NodeParameter : Node
    {
        public Node  Type;
        public Token Name;

        public override string GetString => $"{Type.GetString}{Name.Content}";
        public override Token  GetToken  => Name;

        // Resolve symbols
        public override void ResolveSymbols(Input input)
        {
            // Add local symbol
            input.Locals[Name.Content] = new Symbol(SymbolKind.Parameter, input.Module, Name.Content, this);
        }

        // Type checking
        public override Type TypeCheck(Input input)
        {
            return Type.TypeCheck(input);
        }
    }
}