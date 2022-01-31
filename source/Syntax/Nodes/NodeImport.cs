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
            if (!Config.HasModule(ModuleName.GetString))
            {
                input.Logger.Error(GetToken, "This module was not found.");
                return;
            }

            // Make sure that module isn't the global one
            if (ModuleName.GetString == "global")
            {
                input.Logger.Error(GetToken, "You can't import the global module.");
                return;
            }

            // Add module
            input.Imports.Add(Config.Modules[ModuleName.GetString]);
        }
    }
}