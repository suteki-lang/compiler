using System;
using System.Collections.Generic;

namespace Suteki
{
    class Node
    {
        /*
            Base methods
        */

        public virtual void RegisterSymbols(Input input)
        {

        }

        public virtual void CheckSymbols(Input input)
        {
            
        }

        public virtual ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Void;
        }

        public virtual void Optimize(Input input)
        {
            
        }

        public virtual string ToString()
        {
            return null;
        }

        public virtual void Emit(Input input)
        {

        }
    }

    partial class NodeExport : Node
    {
        public string ModuleName;
    }

    partial class NodeImport : Node
    {
        public string ModuleName;
    }

    partial class NodePrimitive : Node
    {
        public PrimitiveKind Kind;
    }

    partial class NodeBool : Node
    {
        public Token Value;

        // NodeBool Constructor
        public NodeBool(Token value)
        {
            Value = value;
        }
    }

    partial class NodeInteger : Node
    {
        public Token Value;

        // NodeInteger Constructor
        public NodeInteger(Token value)
        {
            Value = value;
        }
    }

    partial class NodeFloat : Node
    {
        public Token Value;

        // NodeFloat Constructor
        public NodeFloat(Token value)
        {
            Value = value;
        }
    }

    partial class NodeString : Node
    {
        public Token Value;

        // NodeString Constructor
        public NodeString(Token value)
        {
            Value = value;
        }
    }

    partial class NodeFunction : Node
    {
        public Node       Type;
        public Token      Name;
        public List<Node> Parameters = new List<Node>();
        public Node       Block;
    }

    partial class NodeParameter : Node
    {
        public Node   Type;
        public Token  Name;
    }

    partial class NodeBlock : Node
    {
        public List<Node> Statements = new List<Node>();
    }

    partial class NodeReturn : Node
    {
        public Node  Expression;
        public Token Start;
    }
}