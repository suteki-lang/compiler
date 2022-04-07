namespace Suteki.Compiler;

/// <summary>
/// A node that represents a primitive type.
/// </summary>
public class NodePrimitiveType : Node
{
    /// <summary>
    /// The kind of the primitive.
    /// </summary>
    public PrimitiveKind Kind;

    /// <summary>
    /// Constructs a <see cref="NodePrimitiveType"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="kind"    >The primitive kind.      </param>
    public NodePrimitiveType(FileLocation location, PrimitiveKind kind)
    {
        Location = location;
        Kind     = kind;
    }

    /// <summary>
    /// Get the name of a node.
    /// </summary>
    public override string GetName()
    {
        return Kind.ToString().ToLower();
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}