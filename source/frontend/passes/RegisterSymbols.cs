using System.Collections.Generic;

namespace Suteki
{
    partial class NodeImport : Node
    {
        public override void RegisterSymbols(Input input)
        {
            // Find module
            Input module = Config.Inputs.Find((m) => m.Module == ModuleName);

            if (module == null)
            {
                input.Logger.Error(Start, "This module was not found."); 
                return;
            }

            // Make sure module symbols was registered  
            if (!module.SymbolsAreRegistered)
            {
                module.SymbolsAreRegistered = true;
                
                foreach (Node node in module.Nodes)
                {
                    node.RegisterSymbols(module);
                }
            }

            foreach (KeyValuePair<string, Symbol> pair in module.Globals)
            {
                // Check for existing symbol
                if (input.Globals.ContainsKey(pair.Key))
                {
                    module.Logger.Error("This symbol was already declared.");
                    break;
                }

                // Add symbol
                input.Globals.Add(pair.Key, pair.Value);
            }
        }
    }

    partial class NodeFunction : Node
    {
        public override void RegisterSymbols(Input input)
        {
            // Check for existing symbol
            if (input.Globals.ContainsKey(Name.Data.ToString()))
            {
                input.Logger.Error(Name, "This symbol was already declared.");
                return;
            }

            // Add symbol
            input.Globals.Add(Name.Data.ToString(), new Symbol(SymbolKind.Function));
        }
    }
}