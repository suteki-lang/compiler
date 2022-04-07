namespace Suteki.Compiler;

/// <summary>
/// A node that represents an import.
/// </summary>
public class NodeImport : Node
{
    /// <summary>
    /// The module to import.
    /// </summary>
    public Node Module;

    /// <summary>
    /// Constructs a <see cref="NodePrimitiveType"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="kind"    >The primitive kind.      </param>
    public NodeImport(FileLocation location, Node module)
    {
        Location = location;
        Module   = module;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}