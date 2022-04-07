namespace Suteki.Compiler;

/// <summary>
/// A node that represents a qualified name.
/// </summary>
public class NodeQualifiedName : Node
{
    /// <summary>
    /// The left name.
    /// </summary>
    public Node Left;

    /// <summary>
    /// The right name.
    /// </summary>
    public Node Right;

    /// <summary>
    /// Constructs a <see cref="NodeQualifiedName"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="left"    >The left name node.      </param>
    /// <param name="right"   >The right name node.     </param>
    public NodeQualifiedName(FileLocation location, Node left, Node right)
    {
        Location = location;
        Left     = left;
        Right    = right;
    }

    /// <summary>
    /// Get the name of a node.
    /// </summary>
    public override string GetName()
    {
        return (Left.GetName() + '.' + Right.GetName());
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}