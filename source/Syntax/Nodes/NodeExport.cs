namespace Suteki
{
    class NodeExport : Node
    {
        public Node ModuleName;

        public override string GetString => ModuleName.GetString;
        public override Token  GetToken  => ModuleName.GetToken;
    }
}