namespace Suteki.Compiler;

/// <summary>
/// A node that represents a constant type.
/// </summary>
public class NodeConstType : Node
{
    /// <summary>
    /// The constant type.
    /// </summary>
    public Node Type;

    /// <summary>
    /// Constructs a <see cref="NodeConstType"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="type"    >The constant type.       </param>
    public NodeConstType(FileLocation location, Node type)
    {
        Location = location;
        Type     = type;
    }

    /// <summary>
    /// Get the name of a node.
    /// </summary>
    public override string GetName()
    {
        return ("const " + Type.GetName());
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}