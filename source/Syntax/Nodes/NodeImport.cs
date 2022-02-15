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
            if (ModuleName.GetString == input.Module.Name)
            {
                // NOTE: maybe this error can be better?
                input.Logger.Error(GetToken, "You can't self import a module.");
                return;
            }

            // Add module
            input.Imports.Add(Config.Modules[ModuleName.GetString]);
        }
    }
}