namespace Suteki.Compiler;

/// <summary>
/// A node that represents a pointer type.
/// </summary>
public class NodePointerType : Node
{
    /// <summary>
    /// The base type.
    /// </summary>
    public Node Base;

    /// <summary>
    /// Constructs a <see cref="NodePointerType"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="baseType">The base type.           </param>
    public NodePointerType(FileLocation location, Node baseType)
    {
        Location = location;
        Base     = baseType;
    }

    /// <summary>
    /// Get the name of a node.
    /// </summary>
    public override string GetName()
    {
        return Base.GetName() + '*';
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}