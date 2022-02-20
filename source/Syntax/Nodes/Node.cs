namespace Suteki;

class Node
{
    // Register symbols
    public virtual void RegisterSymbols(Input input)
    {

    }

    // Resolve symbols
    public virtual void ResolveSymbols(Input input)
    {
        
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

    public virtual string   GetString => "";
    public virtual Token    GetToken  => null;
    public virtual NodeKind Kind      => NodeKind.Node;
}