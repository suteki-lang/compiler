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
            // Check for symbol
            if (!input.Find(Name))
                input.Logger.Error(Name, "This symbol does not exists.");
        }
    }

    partial class NodeReturn : Node
    {
        public override void ResolveSymbols(Input input)
        {
            
        }
    }
}