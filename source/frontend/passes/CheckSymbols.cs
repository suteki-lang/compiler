namespace Suteki
{
    partial class NodeFunction : Node
    {
        public override void CheckSymbols(Input input)
        {
            if (Block != null)
                Block.CheckSymbols(input);
        }
    }

    partial class NodeParameter : Node
    {
        public override void CheckSymbols(Input input)
        {
            
        }
    }

    partial class NodeBlock : Node
    {
        public override void CheckSymbols(Input input)
        {
            foreach (Node node in Statements)
                node.CheckSymbols(input);
        }
    }

    partial class NodeCall : Node
    {
        public override void CheckSymbols(Input input)
        {
            // Check for symbol
            if (!input.Globals.ContainsKey(Name.Content))
                input.Logger.Error(Name, "This symbol does not exists.");
        }
    }

    partial class NodeReturn : Node
    {
        public override void CheckSymbols(Input input)
        {
            
        }
    }
}