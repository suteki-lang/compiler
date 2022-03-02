namespace Suteki.Compiler;

public enum SymbolKind
{
    None,
    Function,
    Parameter,
}

public class Symbol
{
    public SymbolKind   Kind;
    public PropertyKind Property;
    public Module       Module;
    public Type         Type;
    public string       Name;

    // Constructor
    public Symbol(SymbolKind kind, PropertyKind property, Module module, string name)
    {
        Kind     = kind;
        Property = property;
        Module   = module;
        Type     = null;
        Name     = name;
    }
}