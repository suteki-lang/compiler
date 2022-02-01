namespace Suteki
{
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
        public virtual ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Void;
        }

        // Emit C++ code
        public virtual void Emit(Input input)
        {

        }

        public virtual bool     IsPointer => false;
        public virtual string   GetString => "";
        public virtual Token    GetToken  => null;
    }
}