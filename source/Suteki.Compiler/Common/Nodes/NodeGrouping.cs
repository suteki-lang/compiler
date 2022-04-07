namespace Suteki.Compiler;

/// <summary>
/// A node that represents a grouping expression.
/// </summary>
public class NodeGrouping : Node
{
    /// <summary>
    /// The expression.
    /// </summary>
    public Node Expression;

    /// <summary>
    /// Constructs a <see cref="NodeBinary"/> class.
    /// </summary>
    /// <param name="location"  >The location of the node.</param>
    /// <param name="expression">The expression.          </param>
    public NodeGrouping(FileLocation location, Node expression)
    {
        Location   = location;
        Expression = expression;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}