namespace Suteki
{
    class NodeImport : Node
    {
        public Node ModuleName;

        public override string GetString => ModuleName.GetString;
        public override Token  GetToken  => ModuleName.GetToken;

        // Register symbols
        public override void RegisterSymbols(Input input)
        {
            // Find module
            if (!Config.Modules.ContainsKey(ModuleName.GetString))
            {
                input.Logger.Error(GetToken, "This module was not found.");
                return;
            }

            // Add module
            input.Imports.Add(Config.Modules[ModuleName.GetString]);
        }
    }
}