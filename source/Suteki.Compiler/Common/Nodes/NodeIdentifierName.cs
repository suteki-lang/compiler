namespace Suteki.Compiler;

/// <summary>
/// A node that represents a identifier name.
/// </summary>
public class NodeIdentifierName : Node
{
    /// <summary>
    /// The token of the identifier.
    /// </summary>
    public Token Value;

    /// <summary>
    /// Constructs a <see cref="NodeIdentifierName"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="value"   >The identifier token.    </param>
    public NodeIdentifierName(FileLocation location, Token value)
    {
        Location = location;
        Value    = value;
    }

    /// <summary>
    /// Get the name of a node.
    /// </summary>
    public override string GetName() => Value.Content;

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}