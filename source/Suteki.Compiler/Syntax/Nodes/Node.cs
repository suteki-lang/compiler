namespace Suteki.Compiler;

public class Node
{
    // Register symbols
    public virtual void RegisterSymbols(Input input)
    {

    }

    // Resolve symbols
    public virtual void ResolveSymbols(Input input)
    {
        
    }

    // Resolve types
    public virtual Type ResolveTypes(Input input)
    {
        return null;
    }

    // Type checking
    public virtual Type TypeCheck(Input input)
    {
        return null;   
    }

    // Emit C++ code
    public virtual void Emit(Input input)
    {

    }

    public virtual bool     IsIdentifier  => false;
    public virtual string   GetString     => "";
    public virtual string   GetIdentifier => "";
    public virtual Token    GetToken      => null;
    public virtual NodeKind Kind          => NodeKind.Node;
}