namespace Suteki.Compiler;

/// <summary>
/// A node that represents a return statement.
/// </summary>
public class NodeReturn : Node
{
    /// <summary>
    /// The return value.
    /// </summary>
    public Node Expression;

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}