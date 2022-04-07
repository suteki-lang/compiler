namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// A node that represents a block of statements.
/// </summary>
public class NodeBlock : Node
{
    /// <summary>
    /// The statements.
    /// </summary>
    public List<Node> Statements = new List<Node>();

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}