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
    public string       Name;
    public Node         Node;

    // Constructor
    public Symbol(SymbolKind kind, PropertyKind property, Module module, string name, Node node)
    {
        Kind     = kind;
        Property = property;
        Module   = module;
        Name     = name;
        Node     = node;
    }
}