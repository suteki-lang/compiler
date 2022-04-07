namespace Suteki.Compiler;

/// <summary>
/// A node that represents a null.
/// </summary>
public class NodeNull : Node
{
    /// <summary>
    /// The token of the null.
    /// </summary>
    public Token Value;

    /// <summary>
    /// Constructs a <see cref="NodeNull"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="value"   >The null token.          </param>
    public NodeNull(FileLocation location, Token value)
    {
        Location = location;
        Value    = value;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}