using System.Collections.Generic;

namespace Suteki
{
    partial class NodeImport : Node
    {
        public override void RegisterSymbols(Input input)
        {
            // Find module
            Module module = Config.Modules[ModuleName.AsString()];

            if (module == null)
                input.Logger.Error(Start, "This module was not found.");
            else
                input.Imports.Add(module);
        }
    }

    partial class NodeFunction : Node
    {
        public override void RegisterSymbols(Input input)
        {
            // Check for existing symbol
            if (input.Find(Name))
            {
                input.Logger.Error(Name, "This symbol was already declared.");
                return;
            }

            // Add symbol
            input.Module.Symbols.Add(Name.Content, new Symbol(Property, SymbolKind.Function, this));
        }
    }
}