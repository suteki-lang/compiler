namespace Suteki.Compiler;

/// <summary>
/// A node that represents a binary expression.
/// </summary>
public class NodeBinary : Node
{
    /// <summary>
    /// The left expression.
    /// </summary>
    public Node Left;

    /// <summary>
    /// The right expression.
    /// </summary>
    public Node Right;

    /// <summary>
    /// The operator.
    /// </summary>
    public Token Operator;

    /// <summary>
    /// Constructs a <see cref="NodeBinary"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="left"    >The left expression.     </param>
    /// <param name="op"      >The operator.            </param>
    /// <param name="right"   >The right expression.    </param>
    public NodeBinary(FileLocation location, Node left, Token op, Node right)
    {
        Location = location;
        Left     = left;
        Operator = op;
        Right    = right;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}