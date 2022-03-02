namespace Suteki.Compiler;

public class NodeImport : Node
{
    public Node ModuleName;
    public bool IsPublic;

    public override string GetString => ModuleName.GetString;
    public override Token  GetToken  => ModuleName.GetToken;
    public override NodeKind Kind    => NodeKind.Import;

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
        Module module = Config.Modules[ModuleName.GetString];

        if (IsPublic)
            input.Module.Imports.Add(module);
        else
            input.Imports.Add(module);
    }
}