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

        public virtual void ResolveSymbols(Input input)
        {
            
        }

        public virtual ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Void;
        }

        public virtual void Optimize(Input input)
        {
            
        }

        public virtual string AsString()
        {
            return null;
        }

        public virtual void Emit(Input input)
        {

        }
    }

    partial class NodeImport : Node
    {
        public Node   ModuleName;
        public Token  Start;
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

    partial class NodeIdentifierName : Node
    {
        public Token Value;

        // NodeIdentifierName Constructor
        public NodeIdentifierName(Token value)
        {
            Value = value;
        }

        // To string
        public override string AsString()
        {
            return Value.Content;
        }
    }

    partial class NodeQualifiedName : Node
    {
        public Node Left;
        public Node Right;

        // NodeQualifiedName Constructor
        public NodeQualifiedName(Node left, Node right)
        {
            Left  = left;
            Right = right;
        }

        // To string
        public override string AsString()
        {
            return $"{Left.AsString()}.{Right.AsString()}";
        }
    }

    partial class NodeFunction : Node
    {
        public PropertyKind Property;
        public Node         Type;
        public Token        Name;
        public List<Node>   Parameters = new List<Node>();
        public Node         Block;
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

    partial class NodeCall : Node
    {
        public Token      Name;
        public List<Node> Parameters = new List<Node>();
    }

    partial class NodeReturn : Node
    {
        public Node  Expression;
        public Token Start;
    }
}