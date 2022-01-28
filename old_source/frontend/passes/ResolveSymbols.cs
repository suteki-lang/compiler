namespace Suteki
{
    partial class NodeFunction : Node
    {
        public override void ResolveSymbols(Input input)
        {
            if (Block != null)
                Block.ResolveSymbols(input);
        }
    }

    partial class NodeParameter : Node
    {
        public override void ResolveSymbols(Input input)
        {
            
        }
    }

    partial class NodeBlock : Node
    {
        public override void ResolveSymbols(Input input)
        {
            foreach (Node node in Statements)
                node.ResolveSymbols(input);
        }
    }

    partial class NodeCall : Node
    {
        public override void ResolveSymbols(Input input)
        {
            Symbol symbol = input.GetSymbol(Name);

            // Check for symbol
            if (symbol == null)
                input.Logger.Error("This symbol does not exists.");
        }
    }

    partial class NodeReturn : Node
    {
        public override void ResolveSymbols(Input input)
        {
            
        }
    }
}